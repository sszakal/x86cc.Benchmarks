using MediatR;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.MediatR;

public sealed class GetBlogPostHandler(IBlogPostService service, IBlogPostMapper mapper)
    : IRequestHandler<GetBlogPostRequest, BlogPostResponse>
{
    public async Task<BlogPostResponse> Handle(GetBlogPostRequest request, CancellationToken cancellationToken)
    {
        var post = await service.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
                   ?? BlogPostFactory.Create("Missing", "", "", []);
        return mapper.Map(post);
    }
}
