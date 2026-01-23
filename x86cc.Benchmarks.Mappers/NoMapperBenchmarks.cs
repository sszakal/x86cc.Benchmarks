using x86cc.Benchmarks.Mappers.Config;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers;

public class NoMapperBenchmarks : MapperBenchmark
{
    protected override TDest Map<TDest>(CustomerOrderRoot source)
    {
        if (typeof(TDest) == typeof(CustomerOrderRootDto))
        {
            return (TDest)(object)NoMapper.Map(source);
        }

        throw new NotSupportedException($"Unsupported destination type: {typeof(TDest).Name}");
    }

    protected override CustomerOrderRootDto[] MapMany(CustomerOrderRoot[] source)
    {
        return NoMapper.Map(source);
    }
}
