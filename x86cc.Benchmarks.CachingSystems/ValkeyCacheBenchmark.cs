using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace x86cc.Benchmarks.CachingSystems;

public class ValkeyCacheBenchmark : CacheBenchmark
{
    protected override IContainer BuildContainer()
    {
        var builder = new ContainerBuilder("valkey/valkey:9.0.1-alpine")
            .WithPortBinding(6379, true);

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    protected override string GetConnectionString(IContainer container)
    {
        return $"{container.Hostname}:{container.GetMappedPublicPort(6379)}";
    }
}
