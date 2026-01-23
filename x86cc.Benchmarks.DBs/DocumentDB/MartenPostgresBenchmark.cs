using DotNet.Testcontainers.Containers;
using Marten;
using Npgsql;
using System.Linq.Expressions;
using JasperFx;
using Testcontainers.PostgreSql;

namespace x86cc.Benchmarks.DBs.DocumentDB;

public class MartenPostgresBenchmark : DocumentDbBenchmark
{
    private const string DatabaseName = "benchmarks";
    private const string Username = "bench";
    private const string Password = "bench_pw";

    private IDocumentStore? _store;
    private string? _connectionString;

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

    private string GetConnectionString(IContainer container)
    {
        return ((PostgreSqlContainer)container).GetConnectionString();
    }

    protected override Task InitializeAsync(IContainer container)
    {
        _connectionString = GetConnectionString(container);
        _store = DocumentStore.For(options =>
        {
            options.Connection(_connectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.Schema.For<CustomerOrderRoot>()
                .Identity(x => x.Id)
                .Index(x => x.Status)
                .Index(x => x.CreatedAt);
        });

        return Task.CompletedTask;
    }

    protected override async Task ResetDatabaseAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync().ConfigureAwait(false);
        await using (var command = connection.CreateCommand())
        {
            command.CommandText = "DROP SCHEMA IF EXISTS public CASCADE; CREATE SCHEMA public;";
            await command.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        await _store!.Storage.ApplyAllConfiguredChangesToDatabaseAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteInsertAsync(CustomerOrderRoot item)
    {
        await using var session = _store!.LightweightSession();
        session.Store(item);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteInsertBulkAsync(CustomerOrderRoot[] items)
    {
        await using var session = _store!.LightweightSession();
        foreach (var item in items)
        {
            session.Store(item);
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        await using var session = _store!.LightweightSession();
        session.Store(order);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        await using var session = _store!.LightweightSession();
        foreach (var order in orders)
        {
            session.Store(order);
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteSearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        await using var session = _store!.LightweightSession();
        var results = await session.Query<CustomerOrderRoot>()
            .Where(filter)
            .OrderBy(o => o.CreatedAt)
            .Take(take)
            .ToListAsync()
            .ConfigureAwait(false);
        
        if (results.Count != 0) return;
        throw new Exception("No results found");
    }

    protected override async Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id)
    {
        await using var session = _store!.LightweightSession();
        return await session.LoadAsync<CustomerOrderRoot>(id).ConfigureAwait(false);
    }

    protected override async Task ExecuteDeleteAsync(CustomerOrderRoot order)
    {
        await using var session = _store!.LightweightSession();
        session.Delete(order);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task DeleteManyAsync(CustomerOrderRoot[] orders)
    {
        await using var session = _store!.LightweightSession();
        
        foreach (var order in orders)
        {
            session.Delete(order);
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }
}
