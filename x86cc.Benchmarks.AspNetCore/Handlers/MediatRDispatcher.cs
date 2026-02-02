using MediatR;

namespace x86cc.Benchmarks.AspNetCore.Handlers;

public sealed class MediatRDispatcher(IMediator mediator) : IDispatcher
{
    public Task<TResponse> SendAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
    {
        return mediator.Send((IRequest<TResponse>)request, cancellationToken);
    }
}
