using MongoDB.Driver;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Specifications;

namespace x86cc.Benchmarks.AspNetCore.Repositories;

public sealed class MongoBlogPostRepository : IBlogPostRepository
{
    private readonly IMongoCollection<BlogPost> _collection;

    public MongoBlogPostRepository(IMongoCollection<BlogPost> collection)
    {
        _collection = collection;
    }

    public async Task<BlogPost> CreateAsync(BlogPost post, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(post, cancellationToken: cancellationToken).ConfigureAwait(false);
        return post;
    }

    public async Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _collection.Find(post => post.Id == id)
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<IReadOnlyList<BlogPost>> SearchAsync(ISpecification<BlogPost> specification, CancellationToken cancellationToken)
    {
        var results = await _collection.Find(specification.Criteria)
            .Limit(specification.Take)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return results;
    }

    public async Task<BlogPost?> UpdateAsync(BlogPost post, CancellationToken cancellationToken)
    {
        await _collection.ReplaceOneAsync(x => x.Id == post.Id, post, cancellationToken: cancellationToken)
            .ConfigureAwait(false);
        return post;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken)
            .ConfigureAwait(false);
        return result.DeletedCount > 0;
    }
}
