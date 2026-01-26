using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class MicroResolverBenchmark: IoCBenchmark
{
    protected override bool SupportsBasic => true;

    protected override bool SupportsMultiple => true;

    protected override bool SupportsPropertyInjection => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new MicroResolverContainerAdapter();
    }
}