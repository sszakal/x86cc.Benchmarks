using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace x86cc.Benchmarks.DBs.EF;

public class PostgresEfBenchmarkCommon : EFBenchmarkCommon
{
    private const string DatabaseName = "benchmarks";
    private const string Username = "bench";
    private const string Password = "bench_pw";

    protected override IContainer BuildContainer()
    {
        var builder = new PostgreSqlBuilder("postgres:18")
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
        return ((PostgreSqlContainer)container).GetConnectionString();
    }

    protected override DbContextOptions<CustomerOrderDbContext> CreateOptions(string connectionString)
    {
        return new DbContextOptionsBuilder<CustomerOrderDbContext>()
            .UseNpgsql(connectionString)
            .EnableDetailedErrors(false)
            .Options;
    }

}
