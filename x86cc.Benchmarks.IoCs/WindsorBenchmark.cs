using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class WindsorBenchmark: IoCBenchmark
{
    protected override bool SupportsPropertyInjection => true;

    protected override bool SupportGeneric => true;

    protected override bool SupportsMultiple => true;

    protected override bool SupportsInterception => true;

    protected override bool SupportsChildContainer => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new WindsorContainerAdapter();
    }
}