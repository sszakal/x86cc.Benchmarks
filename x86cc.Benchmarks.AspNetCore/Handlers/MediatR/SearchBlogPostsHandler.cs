using MediatR;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.MediatR;

public sealed class SearchBlogPostsHandler(IBlogPostService service, IBlogPostMapper mapper)
    : IRequestHandler<SearchBlogPostsRequest, BlogPostSearchResponse>
{
    public async Task<BlogPostSearchResponse> Handle(SearchBlogPostsRequest request, CancellationToken cancellationToken)
    {
        var results = await service.SearchAsync(request.Query, request.Take, cancellationToken).ConfigureAwait(false);
        var response = new BlogPostSearchResponse
        {
            Items = mapper.MapMany(results)
        };

        return response;
    }
}
