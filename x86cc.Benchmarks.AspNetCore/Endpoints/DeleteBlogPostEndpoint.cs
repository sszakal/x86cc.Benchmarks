using FastEndpoints;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Endpoints;

public sealed class DeleteBlogPostEndpoint(IDispatcher dispatcher) : Endpoint<DeleteBlogPostRequest, DeleteBlogPostResponse>
{
    public override void Configure()
    {
        Delete("/api/blogposts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteBlogPostRequest request, CancellationToken cancellationToken)
    {
        var response = await dispatcher.SendAsync<DeleteBlogPostRequest, DeleteBlogPostResponse>(request, cancellationToken).ConfigureAwait(false);
        await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
    }
}
