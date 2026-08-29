using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Mappers;

public sealed class MapperlyBlogPostMapper : IBlogPostMapper
{
    private readonly BlogPostMapperlyMapper _mapper;

    public MapperlyBlogPostMapper(BlogPostMapperlyMapper mapper)
    {
        _mapper = mapper;
    }

    public BlogPostResponse Map(BlogPost post)
    {
        return _mapper.Map(post);
    }

    public BlogPostResponse[] MapMany(IEnumerable<BlogPost> posts)
    {
        return _mapper.MapMany(posts.ToArray());
    }
}
