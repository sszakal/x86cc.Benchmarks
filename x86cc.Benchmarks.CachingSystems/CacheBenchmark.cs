using BenchmarkDotNet.Attributes;
using StackExchange.Redis;
using System.Runtime.InteropServices;
using System.Text.Json;
using DotNet.Testcontainers.Containers;
using x86cc.Benchmarks.Common;

namespace x86cc.Benchmarks.CachingSystems;

[BenchmarkCategory("Caching Systems")]
[MemoryDiagnoser]
[GcServer(true)]
[Config(typeof(DefaultBenchmarkConfig))]
public abstract class CacheBenchmark : IAsyncDisposable
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private IContainer? _container;
    private ConnectionMultiplexer? _connection;
    private IDatabase? _database;
    private CustomerOrderRoot[] _createData = [];
    private CustomerOrderRoot[] _seedData = [];
    private CustomerOrderRoot[] _updateData = [];
    private int _iterationIndex;

    [Params(1000)]
    public int TestDataCount { get; set; }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _container = BuildContainer();
        await _container.StartAsync().ConfigureAwait(false);

        var connectionString = GetConnectionString(_container);
        _connection = await ConnectWithRetryAsync(connectionString).ConfigureAwait(false);
        _database = _connection.GetDatabase();

        await ResetDatabaseAsync().ConfigureAwait(false);

        var faker = BenchmarkBogusConfig.CreateCustomerOrderRootFaker(seed: 42);
        _createData = faker.Generate(TestDataCount).ToArray();
        _seedData = faker.Generate(TestDataCount).ToArray();
        _updateData = faker.Generate(TestDataCount).ToArray();

        for (var i = 0; i < TestDataCount; i++)
        {
            _updateData[i].Id = _seedData[i].Id;
            ApplyUpdate(_updateData[i]);
        }

        await SeedAsync(_seedData).ConfigureAwait(false);
    }

    [IterationSetup(Targets = [nameof(Create), nameof(Read), nameof(Update)])]
    public void IterationSetup()
    {
        _iterationIndex++;
    }

    [IterationSetup(Targets = [nameof(Delete)])]
    public void DeleteSetup()
    {
        _iterationIndex++;
        ResetDatabaseAsync().GetAwaiter().GetResult();
        SeedAsync(_seedData).GetAwaiter().GetResult();
    }

    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public Task Create()
    {
        var item = _createData[_iterationIndex % TestDataCount];
        return _database!.StringSetAsync(BuildKey(item.Id), Serialize(item));
    }

    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public Task Read()
    {
        var item = _seedData[_iterationIndex % TestDataCount];
        return _database!.StringGetAsync(BuildKey(item.Id));
    }

    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public Task Update()
    {
        var item = _updateData[_iterationIndex % TestDataCount];
        return _database!.StringSetAsync(BuildKey(item.Id), Serialize(item));
    }

    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public Task Delete()
    {
        var item = _seedData[_iterationIndex % TestDataCount];
        return _database!.KeyDeleteAsync(BuildKey(item.Id));
    }

    protected abstract IContainer BuildContainer();

    protected abstract string GetConnectionString(IContainer container);

    protected virtual ConfigurationOptions BuildConfiguration(string connectionString)
    {
        var options = ConfigurationOptions.Parse(connectionString);
        options.AbortOnConnectFail = false;
        options.AllowAdmin = true;
        return options;
    }

    private async Task<ConnectionMultiplexer> ConnectWithRetryAsync(string connectionString)
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (true)
        {
            try
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(BuildConfiguration(connectionString))
                    .ConfigureAwait(false);
                await connection.GetDatabase().PingAsync().ConfigureAwait(false);
                return connection;
            }
            catch (Exception) when (DateTime.UtcNow < deadline)
            {
                await Task.Delay(500).ConfigureAwait(false);
            }
        }
    }

    protected virtual async Task ResetDatabaseAsync()
    {
        await _database!.ExecuteAsync("FLUSHDB").ConfigureAwait(false);
    }

    protected async Task SeedAsync(CustomerOrderRoot[] items)
    {
        var pairs = items.Select(item =>
            new KeyValuePair<RedisKey, RedisValue>(BuildKey(item.Id), Serialize(item))).ToArray();
        await _database!.StringSetAsync(pairs).ConfigureAwait(false);
    }

    protected static string BuildKey(Guid id)
    {
        return $"cache:{id:N}";
    }

    protected static string Serialize(CustomerOrderRoot order)
    {
        return JsonSerializer.Serialize(order, JsonOptions);
    }

    protected static void ApplyUpdate(CustomerOrderRoot order)
    {
        order.Status = OrderStatus.Completed;
        order.UpdatedAt = DateTime.UtcNow;
        order.Metadata["updated"] = "true";
    }

    protected static string? ResolveDockerPlatform()
    {
        var overridePlatform =
            Environment.GetEnvironmentVariable("X86CC_DOCKER_PLATFORM") ??
            Environment.GetEnvironmentVariable("DOCKER_DEFAULT_PLATFORM");

        if (!string.IsNullOrWhiteSpace(overridePlatform))
        {
            return overridePlatform;
        }

        return RuntimeInformation.ProcessArchitecture == Architecture.Arm64 ? "linux/arm64" : null;
    }

    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        if (_connection is not null)
        {
            await _connection.CloseAsync().ConfigureAwait(false);
            await _connection.DisposeAsync();
        }

        if (_container is not null)
        {
            await _container.DisposeAsync().ConfigureAwait(false);
        }
    }

    public ValueTask DisposeAsync()
    {
        return _container?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}
