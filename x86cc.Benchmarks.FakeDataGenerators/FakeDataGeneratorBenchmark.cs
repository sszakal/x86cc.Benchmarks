using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using x86cc.Benchmarks.Common;

namespace x86cc.Benchmarks.FakeDataGenerators;

[MemoryDiagnoser]
[SimpleJob(RuntimeMoniker.Net10_0, warmupCount: 2, iterationCount: 10)]
[BenchmarkCategory("Fake Data Generators")]
[Config(typeof(DefaultBenchmarkConfig))]
public abstract class FakeDataGeneratorBenchmark
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Order
    {
        public int OrderId { get; set; }
        public required string Item { get; set; }
        public int Quantity { get; set; }
        public int? LotNumber { get; set; }
    }
    
    public class User(int userId, string ssn)
    {
        public int Id { get; set; } = userId;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? SomethingUnique { get; set; }
        
        public Guid SomeGuid { get; set; }

        public string? Avatar { get; set; }
        public Guid CartId { get; set; }
        public string SSN { get; set; } = ssn;
        public Gender Gender { get; set; }

        public List<Order> Orders { get; set; } = [];
    }

    [Params(10000)]
    public int ObjectCount { get; set; }
    
    [Benchmark(Description = "Generate Single")]
    public User[] Generate()
    {
        var data = new List<User>();
        for (var i = 0; i < ObjectCount; i++)
        {
            data.Add(GenerateSingle()); 
        }
        return data.ToArray();
    }   
    
    [Benchmark(Description = "Generate Many")]
    public User[] GenerateMany()
    {
        return GenerateMany(ObjectCount);
    }

    protected abstract User GenerateSingle();
    
    protected abstract User[] GenerateMany(int count);
}