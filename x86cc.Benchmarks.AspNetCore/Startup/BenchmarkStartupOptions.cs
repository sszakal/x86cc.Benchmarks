namespace x86cc.Benchmarks.AspNetCore.Startup;

public sealed record BenchmarkStartupOptions(
    EndpointStyle Endpoint,
    MediatorKind Mediator,
    IocContainerKind Ioc,
    MapperKind Mapper,
    DataStoreKind DataStore,
    CacheKind Cache);

public enum EndpointStyle
{
    Controllers,
    FastEndpoints
}

public enum MediatorKind
{
    MediatR,
    Wolverine
}

public enum IocContainerKind
{
    Default,
    Lamar,
    DryIoc
}

public enum MapperKind
{
    Mapster,
    Mapperly
}

public enum DataStoreKind
{
    Marten,
    Mongo
}

public enum CacheKind
{
    Disabled,
    Enabled
}
