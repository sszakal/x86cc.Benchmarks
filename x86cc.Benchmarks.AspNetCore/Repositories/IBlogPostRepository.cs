using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Specifications;

namespace x86cc.Benchmarks.AspNetCore.Repositories;

public interface IBlogPostRepository
{
    Task<BlogPost> CreateAsync(BlogPost post, CancellationToken cancellationToken);
    Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<BlogPost>> SearchAsync(ISpecification<BlogPost> specification, CancellationToken cancellationToken);
    Task<BlogPost?> UpdateAsync(BlogPost post, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
