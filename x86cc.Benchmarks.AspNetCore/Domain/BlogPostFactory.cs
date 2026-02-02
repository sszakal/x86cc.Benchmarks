namespace x86cc.Benchmarks.AspNetCore.Domain;

public static class BlogPostFactory
{
    public static BlogPost Create(string title, string body, string author, string[] tags)
    {
        var now = DateTime.UtcNow;
        return new BlogPost
        {
            Id = Guid.NewGuid(),
            Title = title,
            Slug = Slugify(title),
            Body = body,
            Author = author,
            Tags = tags,
            CreatedAt = now,
            UpdatedAt = now
        };
    }

    public static void ApplyUpdate(BlogPost post, string title, string body, string author, string[] tags)
    {
        post.Title = title;
        post.Slug = Slugify(title);
        post.Body = body;
        post.Author = author;
        post.Tags = tags;
        post.UpdatedAt = DateTime.UtcNow;
    }

    private static string Slugify(string value)
    {
        return value.Trim().ToLowerInvariant().Replace(' ', '-');
    }
}
