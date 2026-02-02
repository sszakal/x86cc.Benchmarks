using x86cc.Benchmarks.AspNetCore.Cache;
using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Repositories;
using x86cc.Benchmarks.AspNetCore.Specifications;

namespace x86cc.Benchmarks.AspNetCore.Services;

public sealed class CachedBlogPostService(
    IBlogPostRepository repository,
    ICache<BlogPost> cache) : IBlogPostService
{
    public async Task<BlogPost> CreateAsync(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var post = BlogPostFactory.Create(request.Title, request.Body, request.Author, request.Tags);
        var created = await repository.CreateAsync(post, cancellationToken).ConfigureAwait(false);
        await cache.SetAsync(created, cancellationToken).ConfigureAwait(false);
        return created;
    }

    public async Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var cached = await cache.GetAsync(id, cancellationToken).ConfigureAwait(false);
        if (cached is not null)
        {
            return cached;
        }

        var post = await repository.GetAsync(id, cancellationToken).ConfigureAwait(false);
        if (post is null)
        {
            return null;
        }

        await cache.SetAsync(post, cancellationToken).ConfigureAwait(false);
        return post;
    }

    public async Task<IReadOnlyList<BlogPost>> SearchAsync(string? query, int take, CancellationToken cancellationToken)
    {
        var specification = new BlogPostSearchSpecification(query, take);
        var results = await repository.SearchAsync(specification, cancellationToken).ConfigureAwait(false);
        return results.ToList();
    }

    public async Task<BlogPost> EditAsync(EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        BlogPost? post = await cache.GetAsync(request.Id, cancellationToken).ConfigureAwait(false);

        post ??= await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
                ?? BlogPostFactory.Create(request.Title, request.Body, request.Author, request.Tags);

        BlogPostFactory.ApplyUpdate(post, request.Title, request.Body, request.Author, request.Tags);
        var updated = await repository.UpdateAsync(post, cancellationToken).ConfigureAwait(false);
        var response = updated ?? post;
        await cache.SetAsync(response, cancellationToken).ConfigureAwait(false);

        return response;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await repository.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
        await cache.RemoveAsync(id, cancellationToken).ConfigureAwait(false);
        return deleted;
    }
}
