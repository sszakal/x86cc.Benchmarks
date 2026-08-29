using FastEndpoints;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Endpoints;

public sealed class EditBlogPostEndpoint(IDispatcher dispatcher) : Endpoint<EditBlogPostRequest, BlogPostResponse>
{
    public override void Configure()
    {
        Put("/api/blogposts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        var response = await dispatcher.SendAsync<EditBlogPostRequest, BlogPostResponse>(request, cancellationToken).ConfigureAwait(false);
        await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
    }
}
