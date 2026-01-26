using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class LamarBenchmark: IoCBenchmark
{
    protected override bool SupportsInterception => false;

    protected override bool SupportGeneric => true;

    protected override bool SupportsMultiple => true;

    protected override bool SupportsPropertyInjection => true;

    protected override bool SupportsChildContainer => false;

    protected override bool SupportAspNetCore => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new LamarContainerAdapter();
    }
}