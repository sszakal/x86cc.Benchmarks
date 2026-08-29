namespace x86cc.Benchmarks.AspNetCore.Cache;

public static class CacheKeyBuilder
{
    public static string BlogPost(Guid id) => $"blogpost:{id:N}";
}
