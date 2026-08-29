using Riok.Mapperly.Abstractions;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Models;

namespace x86cc.Benchmarks.AspNetCore.Mappers;

[Mapper]
public partial class BlogPostMapperlyMapper
{
    public partial BlogPostResponse Map(BlogPost post);
    public partial BlogPostResponse[] MapMany(BlogPost[] posts);
}
