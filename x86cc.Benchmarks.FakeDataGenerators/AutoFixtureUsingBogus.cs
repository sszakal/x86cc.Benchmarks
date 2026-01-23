using AutoFixture;
using BenchmarkDotNet.Attributes;
using Bogus;

namespace x86cc.Benchmarks.FakeDataGenerators;

public class AutoFixtureUsingBogus: FakeDataGeneratorBenchmark
{
    private static string[] fruit = ["apple", "banana", "orange", "strawberry", "kiwi"];

    private int orderIds;
    private int userIds;
    
    private Faker<Order>? _bogusOrderGenerator;
    private Faker<User>? _bogusUserGenerator;
    private Fixture? _autoFixtureWithBogus;
    
    [GlobalSetup]
    public void GlobalSetup()
    {
        orderIds = 0;
        userIds = 0;
        _bogusOrderGenerator = new Faker<Order>()
            .StrictMode(true)
            .RuleFor(o => o.OrderId, f => orderIds++)
            .RuleFor(o => o.Item, f => f.PickRandom(fruit))
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
            .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .8f));
        
        _bogusUserGenerator = new Faker<User>()
            .CustomInstantiator(f => new User(userIds++, f.Random.Replace("###-##-####")))
            .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
            .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
            .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
            .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
            .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
            .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")
            .RuleFor(u => u.CartId, f => Guid.NewGuid())
            .RuleFor(u => u.FullName, (f, u) => u.FirstName + " " + u.LastName)
            .RuleFor(u => u.Orders, f => _bogusOrderGenerator.Generate(3).ToList());
        
        _autoFixtureWithBogus = new Fixture();
        _autoFixtureWithBogus.Customize<Order>(c => c
            .FromFactory(() => _bogusOrderGenerator.Generate())
            .OmitAutoProperties()
        );
        
        _autoFixtureWithBogus.Customize<User>(c => c
            .FromFactory(() => _bogusUserGenerator.Generate())
            .OmitAutoProperties()
        );
    }
    
    protected override User GenerateSingle()
    {
        return _autoFixtureWithBogus.Create<User>();
    }

    protected override User[] GenerateMany(int count)
    {
        return _autoFixtureWithBogus.CreateMany<User>(count).ToArray();
    }
}