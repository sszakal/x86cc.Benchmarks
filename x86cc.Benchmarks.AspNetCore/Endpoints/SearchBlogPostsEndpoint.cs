using FastEndpoints;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Endpoints;

public sealed class SearchBlogPostsEndpoint(IDispatcher dispatcher) : Endpoint<SearchBlogPostsRequest, BlogPostSearchResponse>
{
    public override void Configure()
    {
        Get("/api/blogposts/search");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SearchBlogPostsRequest request, CancellationToken cancellationToken)
    {
        var response = await dispatcher.SendAsync<SearchBlogPostsRequest, BlogPostSearchResponse>(request, cancellationToken).ConfigureAwait(false);
        await Send.OkAsync(response, cancellationToken).ConfigureAwait(false);
    }
}
