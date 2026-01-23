using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using x86cc.Benchmarks.Mappers.Config;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers;

[MemoryDiagnoser]
[BenchmarkCategory("Mappers")]
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 2, iterationCount: 20)]
public abstract class MapperBenchmark
{
    private CustomerOrderRoot[]? TestData;

    [Params(10_000)]
    public int ObjectCount { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        Init();
        var faker = BenchmarkBogusConfig.CreateCustomerOrderRootFaker(seed: 42);
        TestData = faker.Generate(ObjectCount).ToArray();
    }

    [Benchmark]
    public CustomerOrderRootDto[] MapOneByOne()
    {
        var result = new List<CustomerOrderRootDto>();
        
        foreach (var data in TestData!)
        {
            result.Add(Map<CustomerOrderRootDto>(data!));
        }

        return result.ToArray();
    }

    [Benchmark]
    public CustomerOrderRootDto[] MapMany()
    {
        return MapMany(TestData!);
    }
    
    protected virtual void Init()
    {
    }

    protected abstract TDest Map<TDest>(CustomerOrderRoot source);

    protected abstract CustomerOrderRootDto[] MapMany(CustomerOrderRoot[] source);
}
