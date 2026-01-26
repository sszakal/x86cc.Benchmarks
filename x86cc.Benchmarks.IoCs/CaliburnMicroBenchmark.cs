using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class CaliburnMicroBenchmark: IoCBenchmark
{
    protected override bool SupportsMultiple => true;

    protected override bool SupportsPropertyInjection => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new CaliburnMicroContainerAdaptor();
    }
}