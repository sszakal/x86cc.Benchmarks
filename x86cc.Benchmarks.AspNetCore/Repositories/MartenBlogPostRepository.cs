using Marten;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Specifications;

namespace x86cc.Benchmarks.AspNetCore.Repositories;

public sealed class MartenBlogPostRepository(IDocumentStore store) : IBlogPostRepository
{
    public async Task<BlogPost> CreateAsync(BlogPost post, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        session.Store(post);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return post;
    }

    public async Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();
        return await session.LoadAsync<BlogPost>(id, cancellationToken).ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<BlogPost>> SearchAsync(ISpecification<BlogPost> specification, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();
        var results = await session.Query<BlogPost>()
            .Where(specification.Criteria)
            .Take(specification.Take)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return results;
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost post, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        session.Store(post);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return post;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await using var session = store.LightweightSession();
        session.Delete<BlogPost>(id);
        await session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        return true;
    }
}
