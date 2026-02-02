using System.Linq.Expressions;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Specifications;

public sealed class BlogPostSearchSpecification : ISpecification<BlogPost>
{
    public BlogPostSearchSpecification(string? query, int take)
    {
        Query = query;
        Take = take <= 0 ? 20 : take;
        Criteria = BuildCriteria(query);
    }

    public string? Query { get; }

    public Expression<Func<BlogPost, bool>> Criteria { get; }

    public int Take { get; }

    private static Expression<Func<BlogPost, bool>> BuildCriteria(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return post => true;
        }

        return post => post.Title.Contains(query) || post.Body.Contains(query);
    }
}
