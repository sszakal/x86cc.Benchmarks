using System.Text.Json;
using StackExchange.Redis;
using x86cc.Benchmarks.AspNetCore.Domain;

namespace x86cc.Benchmarks.AspNetCore.Cache;

public sealed class ValKeyCache<TAggregate>(IConnectionMultiplexer multiplexer) : ICache<TAggregate>
    where TAggregate : class, IAggregate, new()
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IDatabase _database = multiplexer.GetDatabase();

    public async Task<TAggregate?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var value = await _database.StringGetAsync(CacheKeyBuilder.BlogPost(id)).ConfigureAwait(false);
        if (!value.HasValue) return null;
        return JsonSerializer.Deserialize<TAggregate>(value.ToString(), JsonOptions);
    }

    public Task SetAsync(TAggregate response, CancellationToken cancellationToken)
    {
        var payload = JsonSerializer.Serialize(response, JsonOptions);
        return _database.StringSetAsync(CacheKeyBuilder.BlogPost(response.Id), payload, TimeSpan.FromMinutes(5));
    }

    public Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        return _database.KeyDeleteAsync(CacheKeyBuilder.BlogPost(id));
    }
}
