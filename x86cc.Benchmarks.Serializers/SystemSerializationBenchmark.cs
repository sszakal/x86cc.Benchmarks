using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace x86cc.Benchmarks.Serializers;

public class SystemSerializationBenchmark: SerializationBenchmark
{
    private static JsonSerializerOptions _jsonSerializerOptions = new()
    {
        AllowDuplicateProperties = false,
        AllowOutOfOrderMetadataProperties = false,
        AllowTrailingCommas = false,
        DefaultIgnoreCondition = JsonIgnoreCondition.Never,
        IgnoreReadOnlyFields = false,
        IgnoreReadOnlyProperties = false,
        IncludeFields = true,
        NumberHandling = JsonNumberHandling.Strict,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
        PropertyNameCaseInsensitive = false,
        ReadCommentHandling = JsonCommentHandling.Disallow,
        RespectNullableAnnotations = false,
        RespectRequiredConstructorParameters = false,
        UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
        WriteIndented = false
    };

    protected override T Deserialize<T>(byte[] value)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(value, _jsonSerializerOptions)!;
    }

    protected override byte[] Serialize<T>(T value)
    {
        return Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(value, _jsonSerializerOptions)!);
    }
}