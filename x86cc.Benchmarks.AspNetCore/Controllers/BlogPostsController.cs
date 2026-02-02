using Microsoft.AspNetCore.Mvc;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Controllers;

[ApiController]
[Route("api/blogposts")]
public sealed class BlogPostsController(IDispatcher dispatcher) : ControllerBase
{
    [HttpPost]
    public Task<BlogPostResponse> Create([FromBody] CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        return dispatcher.SendAsync<CreateBlogPostRequest, BlogPostResponse>(request, cancellationToken);
    }

    [HttpGet("{id:guid}")]
    public Task<BlogPostResponse> Get(Guid id, CancellationToken cancellationToken)
    {
        var request = new GetBlogPostRequest { Id = id };
        return dispatcher.SendAsync<GetBlogPostRequest, BlogPostResponse>(request, cancellationToken);
    }

    [HttpGet("search")]
    public Task<BlogPostSearchResponse> Search([FromQuery] string? query, [FromQuery] int take,
        CancellationToken cancellationToken)
    {
        var request = new SearchBlogPostsRequest { Query = query, Take = take };
        return dispatcher.SendAsync<SearchBlogPostsRequest, BlogPostSearchResponse>(request, cancellationToken);
    }

    [HttpPut("{id:guid}")]
    public Task<BlogPostResponse> Edit(Guid id, [FromBody] EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        request.Id = id;
        return dispatcher.SendAsync<EditBlogPostRequest, BlogPostResponse>(request, cancellationToken);
    }

    [HttpDelete("{id:guid}")]
    public Task<DeleteBlogPostResponse> Delete(Guid id, CancellationToken cancellationToken)
    {
        var request = new DeleteBlogPostRequest { Id = id };
        return dispatcher.SendAsync<DeleteBlogPostRequest, DeleteBlogPostResponse>(request, cancellationToken);
    }
}
