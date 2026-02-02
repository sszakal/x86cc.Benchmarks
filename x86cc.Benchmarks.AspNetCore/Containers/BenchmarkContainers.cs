using System.Runtime.InteropServices;
using Docker.DotNet.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Testcontainers.MongoDb;
using Testcontainers.PostgreSql;
using x86cc.Benchmarks.AspNetCore.Containers.LavinMq;

namespace x86cc.Benchmarks.AspNetCore.Containers;

public static class BenchmarkContainers
{
    private const string PostgresDatabase = "benchmarks";
    private const string PostgresUser = "bench";
    private const string PostgresPassword = "bench_pw";

    private static readonly SemaphoreSlim InitLock = new(1, 1);
    private static bool _initialized;

    private static PostgreSqlContainer? _postgres;
    private static MongoDbContainer? _mongo;
    private static IContainer? _valkey;
    private static LavinMqContainer? _lavinMq;

    public static string PostgresConnectionString { get; private set; } = string.Empty;
    public static string MongoConnectionString { get; private set; } = string.Empty;
    public static string ValkeyConnectionString { get; private set; } = string.Empty;
    public static string LavinMqConnectionString { get; private set; } = string.Empty;

    public static async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        await InitLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_initialized)
            {
                return;
            }

            _postgres = new PostgreSqlBuilder("postgres:18.1")
                .WithDatabase(PostgresDatabase)
                .WithUsername(PostgresUser)
                .WithPassword(PostgresPassword)
                .WithCreateParameterModifier(ApplyPlatformIfNeeded)
                .Build();

            _mongo = new MongoDbBuilder("mongo:8.2.3-noble")
                .WithCreateParameterModifier(ApplyPlatformIfNeeded)
                .Build();

            _valkey = new ContainerBuilder("valkey/valkey:9.0.1-alpine")
                .WithPortBinding(6379, true)
                .WithCreateParameterModifier(ApplyPlatformIfNeeded)
                .Build();

            _lavinMq = new LavinMqBuilder()
                .WithCreateParameterModifier(ApplyPlatformIfNeeded)
                .Build();

            await _postgres.StartAsync().ConfigureAwait(false);
            await _mongo.StartAsync().ConfigureAwait(false);
            await _valkey.StartAsync().ConfigureAwait(false);
            await _lavinMq.StartAsync().ConfigureAwait(false);

            PostgresConnectionString = _postgres.GetConnectionString();
            MongoConnectionString = _mongo.GetConnectionString();
            ValkeyConnectionString = $"{_valkey.Hostname}:{_valkey.GetMappedPublicPort(6379)}";
            LavinMqConnectionString = _lavinMq.GetConnectionString();

            _initialized = true;
        }
        finally
        {
            InitLock.Release();
        }
    }

    public static async Task DisposeAsync()
    {
        if (_postgres is not null)
        {
            await _postgres.DisposeAsync().ConfigureAwait(false);
        }

        if (_mongo is not null)
        {
            await _mongo.DisposeAsync().ConfigureAwait(false);
        }

        if (_valkey is not null)
        {
            await _valkey.DisposeAsync().ConfigureAwait(false);
        }

        if (_lavinMq is not null)
        {
            await _lavinMq.DisposeAsync().ConfigureAwait(false);
        }
    }

    private static void ApplyPlatformIfNeeded(CreateContainerParameters parameters)
    {
        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            parameters.Platform = platform;
        }
    }

    private static string? ResolveDockerPlatform()
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
}
