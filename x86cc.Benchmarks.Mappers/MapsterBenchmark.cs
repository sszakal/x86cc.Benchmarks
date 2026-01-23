using Mapster;
using x86cc.Benchmarks.Mappers.Config;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers;

public class MapsterBenchmark : MapperBenchmark
{
    private TypeAdapterConfig? _config;

    protected override void Init()
    {
        _config = MapsterMappings.CreateConfig();
        _config.Compile();
    }

    protected override TDest Map<TDest>(CustomerOrderRoot source)
    {
        return source.Adapt<TDest>(_config!);
    }

    protected override CustomerOrderRootDto[] MapMany(CustomerOrderRoot[] source)
    {
        return source.Adapt<CustomerOrderRootDto[]>(_config!);
    }
}
