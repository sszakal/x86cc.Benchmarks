using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using x86cc.Benchmarks.Mappers.Config;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers;

public class AutoMapperBenchmark : MapperBenchmark
{
    private IMapper? _mapper;

    protected override void Init()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<BenchmarkProfile>(), NullLoggerFactory.Instance);
        config.CompileMappings();
        _mapper = config.CreateMapper();
    }

    protected override TDest Map<TDest>(CustomerOrderRoot source)
    {
        return _mapper!.Map<TDest>(source);
    }

    protected override CustomerOrderRootDto[] MapMany(CustomerOrderRoot[] source)
    {
        return _mapper!.Map<CustomerOrderRootDto[]>(source);
    }
}
