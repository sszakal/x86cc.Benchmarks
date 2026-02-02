using Wolverine;

namespace x86cc.Benchmarks.AspNetCore.Handlers;

public sealed class WolverineDispatcher(IMessageBus bus) : IDispatcher
{
    public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        return bus.InvokeAsync<TResponse>(request!, cancellationToken);
    }
}
