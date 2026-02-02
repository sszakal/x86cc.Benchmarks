using FastEndpoints;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Endpoints;

public sealed class CreateBlogPostEndpoint(IDispatcher dispatcher) : Endpoint<CreateBlogPostRequest, BlogPostResponse>
{
    public override void Configure()
    {
        Post("/api/blogposts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var response = await dispatcher.SendAsync<CreateBlogPostRequest, BlogPostResponse>(request, cancellationToken).ConfigureAwait(false);
        await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
    }
}
