using MessagePack;

namespace x86cc.Benchmarks.Serializers;

public class MessagePackBenchmarks: SerializationBenchmark
{
    protected override T Deserialize<T>(byte[] value)
    {
        return MessagePackSerializer.Deserialize<T>(value)!;
    }

    protected override byte[] Serialize<T>(T value)
    {
        return MessagePackSerializer.Serialize(value)!;
    }
}