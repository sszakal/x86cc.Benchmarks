using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace x86cc.Benchmarks.Serializers;

[MemoryDiagnoser]
[BenchmarkCategory("Serialization")]
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 5, iterationCount: 40)]
public abstract class SerializationBenchmark
{
    [Params(10000)]
    public int ObjectCount { get; set; }
    
    private CustomerOrderRoot[]? _serializeData;
    private byte[][]? _deserializeData;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        var faker = BenchmarkBogusConfig.CreateCustomerOrderRootFaker(seed: 42);
        _serializeData = faker.Generate(ObjectCount).ToArray();
        _deserializeData = _serializeData
            .Select(Serialize)
            .ToArray();
    }
    
    [Benchmark]
    public byte[][] Serialize()
    {
        var result = new byte[ObjectCount][];
        for (var i = 0; i < ObjectCount; i++)
        {
            result[i] = Serialize(_serializeData![i]);
        }
        return result;
    }
    
    [Benchmark]
    public CustomerOrderRoot[] Deserialize()
    {
        var result = new CustomerOrderRoot[ObjectCount];
        for (var i = 0; i < ObjectCount; i++)
        {
            result[i] = Deserialize<CustomerOrderRoot>(_deserializeData![i])!;
        }
        return result;
    }
    
    protected abstract T? Deserialize<T>(byte[] value);
    
    protected abstract byte[] Serialize<T>(T value);
}
