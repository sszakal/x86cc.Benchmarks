using x86cc.Benchmarks.AspNetCore.Contracts;
using x86cc.Benchmarks.AspNetCore.Domain;

namespace x86cc.Benchmarks.AspNetCore.Services;

public interface IBlogPostService
{
    Task<BlogPost> CreateAsync(CreateBlogPostRequest request, CancellationToken cancellationToken);
    Task<BlogPost?> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<BlogPost>> SearchAsync(string? query, int take, CancellationToken cancellationToken);
    Task<BlogPost> EditAsync(EditBlogPostRequest request, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
