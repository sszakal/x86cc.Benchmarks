namespace x86cc.Benchmarks.AspNetCore.Handlers;

public interface IDispatcher
{
    Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken);
}
