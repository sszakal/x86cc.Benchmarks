using DotNet.Testcontainers.Containers;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;
using Testcontainers.MongoDb;

namespace x86cc.Benchmarks.DBs.DocumentDB;

public class MongoDbBenchmark : DocumentDbBenchmark
{
    private const string DatabaseName = "benchmarks";
    private const string CollectionName = "customer_orders";

    private IMongoDatabase? _database;
    private IMongoCollection<CustomerOrderRoot>? _collection;

    protected override IContainer BuildContainer()
    {
        var builder = new MongoDbBuilder("mongo:8.2.3-noble");

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    private string GetConnectionString(IContainer container)
    {
        return ((MongoDbContainer)container).GetConnectionString();
    }

    protected override Task InitializeAsync(IContainer container)
    {
        var connectionString = GetConnectionString(container);
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(DatabaseName);
        _collection = _database.GetCollection<CustomerOrderRoot>(CollectionName);

        return EnsureMongoReadyAsync(client);
    }

    protected override Task ResetDatabaseAsync()
    {
        return _database!.Client.DropDatabaseAsync(DatabaseName);
    }

    protected override Task ExecuteInsertAsync(CustomerOrderRoot item)
    {
        return _collection!.InsertOneAsync(item);
    }

    protected override Task ExecuteInsertBulkAsync(CustomerOrderRoot[] items)
    {
        return _collection!.InsertManyAsync(items);
    }

    protected override Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        var filter = Builders<CustomerOrderRoot>.Filter.Eq(x => x.Id, order.Id);
        return _collection!.ReplaceOneAsync(filter, order);
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        var models = orders.Select(order =>
            new ReplaceOneModel<CustomerOrderRoot>(Builders<CustomerOrderRoot>.Filter.Eq(x => x.Id, order.Id), order));
        await _collection!.BulkWriteAsync(models).ConfigureAwait(false);
    }

    protected override async Task ExecuteSearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        var results = await _collection!.Find(filter)
            .SortBy(o => o.CreatedAt)
            .Limit(take)
            .ToListAsync()
            .ConfigureAwait(false);
        
        if (results.Count != 0) return;
        throw new Exception("No results found");
    }

    protected override async Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id)
    {
        var filter = Builders<CustomerOrderRoot>.Filter.Eq(x => x.Id, id);
        return await _collection!.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    protected override Task ExecuteDeleteAsync(CustomerOrderRoot order)
    {
        var filter = Builders<CustomerOrderRoot>.Filter.Eq(x => x.Id, order.Id);
        return _collection!.DeleteOneAsync(filter);
    }

    protected override Task DeleteManyAsync(CustomerOrderRoot[] orders)
    {
        var ids = orders.Select(o => o.Id).ToArray();
        var filter = Builders<CustomerOrderRoot>.Filter.In(x => x.Id, ids);
        return _collection!.DeleteManyAsync(filter);
    }

    private async Task EnsureMongoReadyAsync(MongoClient client)
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (true)
        {
            try
            {
                await client.GetDatabase(DatabaseName)
                    .RunCommandAsync((Command<BsonDocument>)"{ping:1}")
                    .ConfigureAwait(false);

                var indexModels = new[]
                {
                    new CreateIndexModel<CustomerOrderRoot>(Builders<CustomerOrderRoot>.IndexKeys.Ascending(x => x.Status)),
                    new CreateIndexModel<CustomerOrderRoot>(Builders<CustomerOrderRoot>.IndexKeys.Ascending(x => x.CreatedAt))
                };
                await _collection!.Indexes.CreateManyAsync(indexModels).ConfigureAwait(false);
                return;
            }
            catch (Exception) when (DateTime.UtcNow < deadline)
            {
                await Task.Delay(500).ConfigureAwait(false);
            }
        }
    }
}
