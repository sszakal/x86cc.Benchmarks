using MediatR;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Contracts;

public sealed class DeleteBlogPostRequest : IRequest<DeleteBlogPostResponse>
{
    public Guid Id { get; set; }
}
