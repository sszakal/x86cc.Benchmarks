using ProtoBuf;

namespace x86cc.Benchmarks.Serializers;

public class ProtoBufBenchmarks: SerializationBenchmark
{
    protected override T Deserialize<T>(byte[] value)
    {
        return Serializer.Deserialize<T>(value)!;
    }

    protected override byte[] Serialize<T>(T value)
    {
        using MemoryStream userInputStream = new MemoryStream();
        Serializer.Serialize(userInputStream, value);
        return userInputStream.ToArray();
    }
}