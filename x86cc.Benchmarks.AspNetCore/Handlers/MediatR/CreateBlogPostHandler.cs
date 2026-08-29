using MediatR;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.MediatR;

public sealed class CreateBlogPostHandler(IBlogPostService service, IBlogPostMapper mapper)
    : IRequestHandler<CreateBlogPostRequest, BlogPostResponse>
{
    public async Task<BlogPostResponse> Handle(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken).ConfigureAwait(false);
        return mapper.Map(created);
    }
}
