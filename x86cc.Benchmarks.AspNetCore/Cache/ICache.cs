using x86cc.Benchmarks.AspNetCore.Domain;

namespace x86cc.Benchmarks.AspNetCore.Cache;

public interface ICache<TAggregate> where TAggregate : class, IAggregate, new()
{
    Task<TAggregate?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task SetAsync(TAggregate value, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}
