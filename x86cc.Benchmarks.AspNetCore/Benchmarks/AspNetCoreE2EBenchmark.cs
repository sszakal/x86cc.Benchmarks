using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Startup;
using x86cc.Benchmarks.AspNetCore.Containers;
using x86cc.Benchmarks.Common;

namespace x86cc.Benchmarks.AspNetCore.Benchmarks;

[BenchmarkCategory("AspNetCore E2E")]
[MemoryDiagnoser]
[GcServer(true)]
[Config(typeof(DefaultBenchmarkConfig))]
public class AspNetCoreE2EBenchmark : IAsyncDisposable
{
    private BenchmarkWebApplicationFactory? _factory;
    private HttpClient? _client;
    private Guid _seedId;
    private int _createIndex;
    private int _editIndex;
    private CreateBlogPostRequest[] _createRequests = [];
    private SearchBlogPostsRequest _searchRequest = new();
    private Guid _deleteId;

    [ParamsSource(nameof(StartupScenarios))]
    public BenchmarkScenario Scenario { get; set; } = new(typeof(FastEndpointsWolverineLamarMapperlyMartenCached));

    public static IEnumerable<BenchmarkScenario> StartupScenarios =>
        BenchmarkStartupMatrix.StartupTypes.Select(type => new BenchmarkScenario(type));

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        await BenchmarkContainers.InitializeAsync().ConfigureAwait(false);

        _factory = new BenchmarkWebApplicationFactory(Scenario.StartupType);
        _client = _factory.CreateClient();

        _createRequests = Enumerable.Range(0, 200)
            .Select(index => new CreateBlogPostRequest
            {
                Title = $"Post {index}",
                Body = "Benchmark content body",
                Author = "benchmarker",
                Tags = ["bench", "e2e"]
            })
            .ToArray();

        _seedId = await SeedAsync(25).ConfigureAwait(false);

        _searchRequest = new SearchBlogPostsRequest
        {
            Query = "Post",
            Take = 25
        };
    }

    [IterationSetup(Targets = [nameof(Delete)])]
    public void DeleteSetup()
    {
        _deleteId = SeedAsync(1).ConfigureAwait(false).GetAwaiter().GetResult();
    }

    [Benchmark]
    public async Task Create()
    {
        var request = _createRequests[_createIndex++ % _createRequests.Length];
        using var response = await _client!.PostAsJsonAsync("/api/blogposts", request).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadFromJsonAsync<BlogPostResponse>().ConfigureAwait(false);
    }

    [Benchmark]
    public async Task Get()
    {
        _ = await _client!.GetFromJsonAsync<BlogPostResponse>($"/api/blogposts/{_seedId}").ConfigureAwait(false);
    }

    [Benchmark]
    public async Task Search()
    {
        var url = $"/api/blogposts/search?query={_searchRequest.Query}&take={_searchRequest.Take}";
        _ = await _client!.GetFromJsonAsync<BlogPostSearchResponse>(url).ConfigureAwait(false);
    }

    [Benchmark]
    public async Task Edit()
    {
        var request = new EditBlogPostRequest
        {
            Id = _seedId,
            Title = $"Updated {_editIndex++}",
            Body = "Updated body",
            Author = "benchmarker",
            Tags = ["bench", "edit"]
        };

        using var response = await _client!.PutAsJsonAsync($"/api/blogposts/{_seedId}", request)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadFromJsonAsync<BlogPostResponse>().ConfigureAwait(false);
    }

    [Benchmark]
    public async Task Delete()
    {
        using var response = await _client!.DeleteAsync($"/api/blogposts/{_deleteId}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        _ = await response.Content.ReadFromJsonAsync<DeleteBlogPostResponse>().ConfigureAwait(false);
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        _client?.Dispose();
        if (_factory is not null)
        {
            await _factory.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await GlobalCleanup().ConfigureAwait(false);
    }

    private async Task<Guid> SeedAsync(int count)
    {
        Guid firstId = Guid.Empty;
        for (var index = 0; index < count; index++)
        {
            var request = new CreateBlogPostRequest
            {
                Title = $"Seeded Post {index}",
                Body = "Seeded content",
                Author = "benchmarker",
                Tags = ["seed"]
            };

            using var response = await _client!.PostAsJsonAsync("/api/blogposts", request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            var created = await response.Content.ReadFromJsonAsync<BlogPostResponse>().ConfigureAwait(false);
            if (index == 0 && created is not null)
            {
                firstId = created.Id;
            }
        }

        return firstId;
    }
}

public sealed record BenchmarkScenario(Type StartupType)
{
    public override string ToString()
    {
        var options = BenchmarkStartupAttribute.GetOptions(StartupType);
        return $"{FormatEndpoint(options.Endpoint)}/{FormatMediator(options.Mediator)}/{FormatIoc(options.Ioc)}/{FormatMapper(options.Mapper)}/{FormatCache(options.Cache)}/{FormatDataStore(options.DataStore)}";
    }

    private static string FormatEndpoint(EndpointStyle endpoint) =>
        endpoint == EndpointStyle.FastEndpoints ? "F" : "C";

    private static string FormatMediator(MediatorKind mediator) =>
        mediator == MediatorKind.Wolverine ? "W" : "M";

    private static string FormatIoc(IocContainerKind ioc) =>
        ioc == IocContainerKind.DryIoc ? "Dr"
        : ioc == IocContainerKind.Lamar ? "L"
        : "MS";

    private static string FormatMapper(MapperKind mapper) =>
        mapper == MapperKind.Mapperly ? "My" : "Mp";

    private static string FormatCache(CacheKind cache) =>
        cache == CacheKind.Enabled ? "C" : "NC";

    private static string FormatDataStore(DataStoreKind dataStore) =>
        dataStore == DataStoreKind.Marten ? "P" : "M";
}
