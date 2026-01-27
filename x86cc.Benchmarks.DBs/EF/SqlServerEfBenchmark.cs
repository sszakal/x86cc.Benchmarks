using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Images;
using DotNet.Testcontainers.Builders;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using Testcontainers.MsSql;

namespace x86cc.Benchmarks.DBs.EF;

public class SqlServerEfBenchmark : EFBenchmarkCommon
{
    private const string DatabaseName = "Benchmarks";
    private const string Username = "sa";
    private const string Password = "Your_strong_password1!";

    protected override IContainer BuildContainer()
    {
        IImage image;
        if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
        {
            var futureImage = new ImageFromDockerfileBuilder()
                .WithName("x86cc.benchmarks.azure-sql-edge:local")
                .WithDockerfileDirectory(CommonDirectoryPath.GetCallerFileDirectory(), "../docker/azure-sql-edge")
                .WithDockerfile("Dockerfile")
                .Build();
            futureImage.CreateAsync().GetAwaiter().GetResult();
            image = futureImage;
        }
        else
        {
            image = new DockerImage("mcr.microsoft.com/mssql/server:2022-latest");
        }

        var builder = new MsSqlBuilder(image)
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
        return ((MsSqlContainer)container).GetConnectionString();
    }

    protected override DbContextOptions<CustomerOrderDbContext> CreateOptions(string connectionString)
    {
        var sqlBuilder = new SqlConnectionStringBuilder(connectionString);
        var masterBuilder = new SqlConnectionStringBuilder(sqlBuilder.ConnectionString)
        {
            InitialCatalog = "master"
        };

        sqlBuilder.InitialCatalog = DatabaseName;

        return new DbContextOptionsBuilder<CustomerOrderDbContext>()
            .UseSqlServer(sqlBuilder.ConnectionString)
            .EnableDetailedErrors(false)
            .Options;
    }
}
