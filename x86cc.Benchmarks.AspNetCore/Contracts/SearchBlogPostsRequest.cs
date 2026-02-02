using MediatR;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Contracts;

public sealed class SearchBlogPostsRequest : IRequest<BlogPostSearchResponse>
{
    public string? Query { get; set; }
    public int Take { get; set; } = 20;
}
