using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Repositories;
using x86cc.Benchmarks.AspNetCore.Specifications;

namespace x86cc.Benchmarks.AspNetCore.Services;

public sealed class BlogPostService(IBlogPostRepository repository) : IBlogPostService
{
    public async Task<BlogPost> CreateAsync(CreateBlogPostRequest request, CancellationToken cancellationToken)
    {
        var post = BlogPostFactory.Create(request.Title, request.Body, request.Author, request.Tags);
        var created = await repository.CreateAsync(post, cancellationToken).ConfigureAwait(false);
        return created;
    }

    public Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return repository.GetAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPost>> SearchAsync(string? query, int take, CancellationToken cancellationToken)
    {
        var specification = new BlogPostSearchSpecification(query, take);
        var results = await repository.SearchAsync(specification, cancellationToken).ConfigureAwait(false);
        return results.ToList();
    }

    public async Task<BlogPost> EditAsync(EditBlogPostRequest request, CancellationToken cancellationToken)
    {
        var post = await repository.GetAsync(request.Id, cancellationToken).ConfigureAwait(false)
                   ?? BlogPostFactory.Create(request.Title, request.Body, request.Author, request.Tags);

        BlogPostFactory.ApplyUpdate(post, request.Title, request.Body, request.Author, request.Tags);
        var updated = await repository.UpdateAsync(post, cancellationToken).ConfigureAwait(false);
        return updated ?? post;
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        return repository.DeleteAsync(id, cancellationToken);
    }
}
