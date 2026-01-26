using x86cc.Benchmarks.IoCs.Adapters;

namespace x86cc.Benchmarks.IoCs;

public class MicrosoftExtensionsDependencyInjectionBenchmark: IoCBenchmark
{
    protected override bool SupportGeneric => true;

    protected override bool SupportsMultiple => true;

    protected override bool SupportAspNetCore => true;
    
    protected override IContainerAdapter BuildContainer()
    {
        return new MicrosoftExtensionsDependencyInjectionContainerAdapter();
    }
}