using System.Text;
using YamlDotNet.Serialization;

namespace x86cc.Benchmarks.Serializers;

public class YamlDotNetBenchmark : SerializationBenchmark
{
    private readonly ISerializer _serializer = new SerializerBuilder().Build();
    private readonly IDeserializer _deserializer = new DeserializerBuilder().Build();

    protected override T Deserialize<T>(byte[] value)
    {
        return _deserializer.Deserialize<T>(Encoding.UTF8.GetString(value));
    }

    protected override byte[] Serialize<T>(T value)
    {
        var yaml = _serializer.Serialize(value);
        return Encoding.UTF8.GetBytes(yaml);
    }
}
