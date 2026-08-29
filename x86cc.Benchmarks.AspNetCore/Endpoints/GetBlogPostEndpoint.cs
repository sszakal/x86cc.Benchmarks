using FastEndpoints;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Endpoints;

public sealed class GetBlogPostEndpoint(IDispatcher dispatcher) : Endpoint<GetBlogPostRequest, BlogPostResponse>
{
    public override void Configure()
    {
        Get("/api/blogposts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBlogPostRequest request, CancellationToken cancellationToken)
    {
        var response = await dispatcher.SendAsync<GetBlogPostRequest, BlogPostResponse>(request, cancellationToken).ConfigureAwait(false);
        await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
    }
}
