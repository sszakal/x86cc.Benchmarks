using MediatR;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Contracts;

public sealed class EditBlogPostRequest : IRequest<BlogPostResponse>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
}
