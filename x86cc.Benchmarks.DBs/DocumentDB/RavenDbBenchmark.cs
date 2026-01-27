using DotNet.Testcontainers.Containers;
using Raven.Client.Documents;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using Testcontainers.RavenDb;

namespace x86cc.Benchmarks.DBs.DocumentDB;

public class RavenDbBenchmark : DocumentDbBenchmark
{
    private const string DatabaseName = "benchmarks";
    private IDocumentStore? _store;
    
    private static string GetImageTag()
    {
        return RuntimeInformation.ProcessArchitecture == Architecture.Arm64
            ? "ravendb/ravendb:7.1-ubuntu-arm64v8-latest"
            : "ravendb/ravendb:7.1-ubuntu-latest";
    }

    protected override IContainer BuildContainer()
    {
        var builder = new RavenDbBuilder(GetImageTag())
            .WithEnvironment("RAVEN_UnsecuredAccessAllowed", "PublicNetwork");

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    private string GetConnectionString(IContainer container)
    {
        return ((RavenDbContainer)container).GetConnectionString();
    }

    protected override async Task InitializeAsync(IContainer container)
    {
        var connectionString = GetConnectionString(container);
        _store = new DocumentStore
        {
            Urls = [connectionString],
            Database = DatabaseName
        };
        _store.Conventions.FindIdentityProperty = memberInfo =>
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.Name == "Id" && propertyInfo.PropertyType == typeof(string);
            }

            return false;
        };
        _store.Conventions.MaxNumberOfRequestsPerSession = 30;
        _store.Initialize();

        await EnsureDatabaseExistsAsync().ConfigureAwait(false);
    }

    protected override async Task ResetDatabaseAsync()
    {
        await _store!.Maintenance.Server.SendAsync(new DeleteDatabasesOperation(DatabaseName, hardDelete: true))
            .ConfigureAwait(false);
        await _store.Maintenance.Server.SendAsync(new CreateDatabaseOperation(new DatabaseRecord(DatabaseName)))
            .ConfigureAwait(false);
    }
    
    protected override async Task ExecuteInsertAsync(CustomerOrderRoot item)
    {
        using var session = _store!.OpenAsyncSession();
        await session.StoreAsync(item, ToDocumentId(item.Id)).ConfigureAwait(false);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteInsertBulkAsync(CustomerOrderRoot[] items)
    {
        using var session = _store!.OpenAsyncSession();
        foreach (var item in items)
        {
            await session.StoreAsync(item, ToDocumentId(item.Id)).ConfigureAwait(false);
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        using var session = _store!.OpenAsyncSession();
        await session.StoreAsync(order, ToDocumentId(order.Id)).ConfigureAwait(false);
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        using var session = _store!.OpenAsyncSession();
        foreach (var order in orders)
        {
            await session.StoreAsync(order, ToDocumentId(order.Id)).ConfigureAwait(false);
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteSearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        using var session = _store!.OpenAsyncSession();
        var results =  await session.Query<CustomerOrderRoot>()
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
        using var session = _store!.OpenAsyncSession();
        return await session.LoadAsync<CustomerOrderRoot>(ToDocumentId(id)).ConfigureAwait(false);
    }

    protected override async Task ExecuteDeleteAsync(CustomerOrderRoot order)
    {
        using var session = _store!.OpenAsyncSession();
        session.Delete(ToDocumentId(order.Id));
        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task DeleteManyAsync(CustomerOrderRoot[] orders)
    {
        using var session = _store!.OpenAsyncSession();
        foreach (var order in orders)
        {
            session.Delete(ToDocumentId(order.Id));
        }

        await session.SaveChangesAsync().ConfigureAwait(false);
    }

    private async Task EnsureDatabaseExistsAsync()
    {
        var databaseExists = await _store!.Maintenance.Server
            .SendAsync(new GetDatabaseRecordOperation(DatabaseName))
            .ConfigureAwait(false);
        if (databaseExists == null)
        {
            await _store.Maintenance.Server
                .SendAsync(new CreateDatabaseOperation(new DatabaseRecord(DatabaseName)))
                .ConfigureAwait(false);
        }
    }

    private static string ToDocumentId(Guid id) => $"orders/{id:N}";
}
