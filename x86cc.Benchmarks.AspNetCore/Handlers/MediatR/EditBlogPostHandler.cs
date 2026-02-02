using MediatR;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.MediatR;

public sealed class EditBlogPostHandler(IBlogPostService service, IBlogPostMapper mapper)
    : IRequestHandler<EditBlogPostRequest, BlogPostResponse>
{
    public async Task<BlogPostResponse> Handle(EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        var updated = await service.EditAsync(request, cancellationToken).ConfigureAwait(false);
        return mapper.Map(updated);
    }
}
