using MongoDB.Bson.Serialization.Attributes;

namespace x86cc.Benchmarks.AspNetCore.Domain;

public sealed class BlogPost: IAggregate
{
    [BsonId]
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
