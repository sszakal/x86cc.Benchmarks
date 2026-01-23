using BenchmarkDotNet.Attributes;
using DotNet.Testcontainers.Containers;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Bogus;
using x86cc.Benchmarks.Common;

namespace x86cc.Benchmarks.DBs.DocumentDB;

[BenchmarkCategory("Document DBs")]
[MemoryDiagnoser]
[GcServer(true)]
[Config(typeof(DefaultBenchmarkConfig))]
public abstract class DocumentDbBenchmark : IAsyncDisposable
{
    private IContainer? _container;
    private CustomerOrderRoot[] TestData { get; set; } = [];
    private CustomerOrderRoot[] SeedData { get; set; } = [];

    private int _iterationIndex = 0;
    private Faker<CustomerOrderRoot>? _faker;

    [Params(2000)]
    public int TestDataCount { get; set; }

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        _container = BuildContainer();
        await _container.StartAsync().ConfigureAwait(false);

        await InitializeAsync(_container).ConfigureAwait(false);
        await ResetDatabaseAsync().ConfigureAwait(false);

        _faker = BenchmarkBogusConfig.CreateCustomerOrderRootFaker(seed: 42);
        TestData = _faker.Generate(TestDataCount).ToArray();
        SeedData = _faker.Generate(TestDataCount).ToArray();
        await ExecuteInsertBulkAsync(SeedData);
    }

    [IterationSetup(Targets = [nameof(Create), nameof(Read), nameof(Update), nameof(Delete)])]
    public void Simple_CRUD_Setup()
    {
        _iterationIndex++;
    }
    
    [Benchmark]
    [WarmupCount(100)]
    [IterationCount(1000)]
    public async Task Create()
    {
        await ExecuteInsertAsync(TestData[_iterationIndex-1]).ConfigureAwait(false);
    }
    
    [Benchmark]
    [WarmupCount(100)]
    [IterationCount(1000)]
    public Task Read()
    {
        return ExecuteLoadByIdAsync(SeedData[_iterationIndex-1].Id);
    }

    [Benchmark]
    [WarmupCount(100)]
    [IterationCount(1000)]
    public Task Update()
    {
        var index = _iterationIndex % TestDataCount;
        var updateObject = TestData[_iterationIndex-1];
        updateObject.Id = SeedData[_iterationIndex-1].Id;
        return ExecuteUpdateAsync(updateObject);
    }
    
    [Benchmark]
    [WarmupCount(100)]
    [IterationCount(1000)]
    public Task Delete()
    {
        return ExecuteDeleteAsync(SeedData[_iterationIndex-1]);
    }
    
    [IterationSetup(Targets = [nameof(Create_Bulk)])]
    public void Create_Bulk_Setup()
    {
        TestData = _faker!.Generate(TestDataCount).ToArray();
    }    

    [Benchmark]
    [WarmupCount(1)]
    [IterationCount(10)]
    public Task Create_Bulk()
    {
        return ExecuteInsertBulkAsync(TestData);
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(100)]
    public Task Read_Search()
    {
        return ExecuteSearchAsync(o => o.Status == OrderStatus.Submitted, TestDataCount);
    }

    [IterationSetup(Targets = [nameof(Update_Bulk)])]
    public void Update_Bulk_Setup()
    {
        for (var index = 0; index < SeedData.Length; index++)
        {
            var order = SeedData[index];
            (order.Id, SeedData[SeedData.Length - 1 - index].Id) = (SeedData[SeedData.Length - 1 - index].Id, order.Id);
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(100)]
    public Task Update_Bulk()
    {
        return ExecuteUpdateBulkAsync(SeedData);
    }

    [IterationSetup(Targets = [nameof(Delete_Bulk)])]
    public void Delete_Bulk_Setup()
    {
        ResetDatabaseAsync().GetAwaiter().GetResult();
        ExecuteInsertBulkAsync(SeedData).GetAwaiter().GetResult();
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(100)]
    public Task Delete_Bulk()
    {
        return DeleteManyAsync(SeedData);
    }

    protected abstract IContainer BuildContainer();

    protected abstract Task InitializeAsync(IContainer container);

    protected abstract Task ResetDatabaseAsync();

    protected abstract Task ExecuteInsertAsync(CustomerOrderRoot item);

    protected abstract Task ExecuteInsertBulkAsync(CustomerOrderRoot[] items);

    protected abstract Task ExecuteUpdateAsync(CustomerOrderRoot order);

    protected abstract Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders);

    protected abstract Task ExecuteSearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take);

    protected abstract Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id);

    protected abstract Task ExecuteDeleteAsync(CustomerOrderRoot order);

    protected abstract Task DeleteManyAsync(CustomerOrderRoot[] orders);

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
        if (_container != null)
        {
            await _container.DisposeAsync().ConfigureAwait(false);
        }
    }

    public ValueTask DisposeAsync()
    {
        return _container?.DisposeAsync() ?? ValueTask.CompletedTask;
    }
}
