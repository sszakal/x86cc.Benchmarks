using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Testcontainers.Oracle;

namespace x86cc.Benchmarks.DBs.EF;

public class OracleEfBenchmark : EFBenchmarkCommon
{
    private const string DatabaseName = "FREEPDB1";
    private const string Username = "bench";
    private const string Password = "bench_pw";

    protected override IContainer BuildContainer()
    {
        var builder = new OracleBuilder("gvenzl/oracle-free:23-slim-faststart")
            .WithUsername(Username)
            .WithPassword(Password)
            .WithDatabase(DatabaseName);

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    protected override string GetConnectionString(IContainer container)
    {
        return ((OracleContainer)container).GetConnectionString();
    }

    protected override DbContextOptions<CustomerOrderDbContext> CreateOptions(string connectionString)
    {
        return new DbContextOptionsBuilder<CustomerOrderDbContext>()
            .UseOracle(connectionString)
            .EnableDetailedErrors(false)
            .Options;
    }

}
