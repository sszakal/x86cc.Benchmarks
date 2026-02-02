using MediatR;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Contracts;

public sealed class GetBlogPostRequest : IRequest<BlogPostResponse>
{
    public Guid Id { get; set; }
}
