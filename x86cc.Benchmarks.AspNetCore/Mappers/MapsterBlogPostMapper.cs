using Mapster;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Mappers;

public sealed class MapsterBlogPostMapper : IBlogPostMapper
{
    public BlogPostResponse Map(BlogPost post)
    {
        return post.Adapt<BlogPostResponse>();
    }

    public BlogPostResponse[] MapMany(IEnumerable<BlogPost> posts)
    {
        return posts.Adapt<BlogPostResponse[]>();
    }
}
