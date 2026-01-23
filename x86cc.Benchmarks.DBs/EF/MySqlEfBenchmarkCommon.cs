using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MySql;

namespace x86cc.Benchmarks.DBs.EF;

public class MySqlEfBenchmarkCommon : EFBenchmarkCommon
{
    private const string DatabaseName = "benchmarks";
    private const string Username = "bench";
    private const string Password = "bench_pw";
    private static readonly Version ServerVersion = new(8, 0, 34);

    protected override IContainer BuildContainer()
    {
        var builder = new MySqlBuilder("mysql:9.6.0")
            .WithDatabase(DatabaseName)
            .WithUsername(Username)
            .WithPassword(Password);

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    protected override string GetConnectionString(IContainer container)
    {
        return ((MySqlContainer)container).GetConnectionString();
    }

    protected override DbContextOptions<CustomerOrderDbContext> CreateOptions(string connectionString)
    {
        return new DbContextOptionsBuilder<CustomerOrderDbContext>()
            .UseMySQL(connectionString)
            .EnableDetailedErrors(false)
            .Options;
    }
}
