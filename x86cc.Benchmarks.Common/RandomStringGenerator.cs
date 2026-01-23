namespace x86cc.Benchmarks.Common;

public static class RandomStringGenerator
{
    private static Random random = new();
    private const string Pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateRandomString(int length)
    {
        // Use a static Random instance to avoid duplicate strings when called quickly
        var chars = Enumerable.Range(0, length)
            .Select(x => Pool[random.Next(0, Pool.Length)]);
        
        return new string(chars.ToArray());
    }
}