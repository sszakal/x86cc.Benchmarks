using AutoFixture;
using BenchmarkDotNet.Attributes;

namespace x86cc.Benchmarks.FakeDataGenerators;

public class AutoFixture: FakeDataGeneratorBenchmark
{
    private Fixture? _autoFixture;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        _autoFixture = new Fixture();
    }
    
    protected override User GenerateSingle()
    {
        return _autoFixture.Create<User>();
    }

    protected override User[] GenerateMany(int count)
    {
        return _autoFixture.CreateMany<User>(count).ToArray();
    }
}