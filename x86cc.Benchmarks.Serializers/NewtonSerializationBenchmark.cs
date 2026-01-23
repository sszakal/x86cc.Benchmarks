using System.Text;

namespace x86cc.Benchmarks.Serializers;

public class NewtonSerializationBenchmark: SerializationBenchmark
{
    protected override T Deserialize<T>(byte[] value)
    {
        return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(value))!;
    }

    protected override byte[] Serialize<T>(T value)
    {
        return Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(value)!);
    }
}