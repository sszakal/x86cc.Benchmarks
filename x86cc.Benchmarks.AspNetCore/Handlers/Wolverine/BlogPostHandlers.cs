using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Handlers.Wolverine;

public sealed class BlogPostHandlers(IBlogPostService service, IBlogPostMapper mapper)
{
    public async Task<BlogPostResponse> Handle(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var created = await service.CreateAsync(request, cancellationToken).ConfigureAwait(false);
        return mapper.Map(created);
    }

    public async Task<BlogPostResponse?> Handle(GetBlogPostRequest request, CancellationToken cancellationToken)
    {
        var post = await service.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);
        return post is null ? null : mapper.Map(post);
    }

    public async Task<BlogPostSearchResponse> Handle(SearchBlogPostsRequest request, CancellationToken cancellationToken)
    {
        var results = await service.SearchAsync(request.Query, request.Take, cancellationToken).ConfigureAwait(false);
        var response = new BlogPostSearchResponse
        {
            Items = mapper.MapMany(results)
        };

        return response;
    }

    public async Task<BlogPostResponse> Handle(EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        var updated = await service.EditAsync(request, cancellationToken).ConfigureAwait(false);
        return mapper.Map(updated);
    }

    public async Task<DeleteBlogPostResponse> Handle(DeleteBlogPostRequest request, CancellationToken cancellationToken)
    {
        var deleted = await service.DeleteAsync(request.Id, cancellationToken).ConfigureAwait(false);

        return new DeleteBlogPostResponse
        {
            Id = request.Id,
            Deleted = deleted
        };
    }
}
