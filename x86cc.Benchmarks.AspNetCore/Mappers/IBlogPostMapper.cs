using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Mappers;

public interface IBlogPostMapper
{
    BlogPostResponse Map(BlogPost post);
    BlogPostResponse[] MapMany(IEnumerable<BlogPost> posts);
}
