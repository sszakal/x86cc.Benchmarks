using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class NoContainerBenchmark: IoCBenchmark
{
    protected override bool SupportsConditional => true;

    protected override bool SupportGeneric => true;

    protected override bool SupportsMultiple => true;

    protected override bool SupportsPropertyInjection => true;

    protected override bool SupportsInterception => true;

    protected override bool SupportsChildContainer => true;

    protected override bool SupportsBasic => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new NoContainerAdapter();
    }
}