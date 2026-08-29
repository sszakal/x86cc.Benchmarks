namespace x86cc.Benchmarks.AspNetCore.Startup;

[BenchmarkStartup(EndpointStyle.Controllers, MediatorKind.Wolverine, IocContainerKind.Lamar, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class ControllersWolverineLamarMapperlyMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.Lamar, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class FastEndpointsWolverineLamarMapperlyMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.Lamar, MapperKind.Mapster, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class FastEndpointsWolverineLamarMapsterMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.Default, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class FastEndpointsWolverineDefaultMapperlyMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.DryIoc, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class FastEndpointsWolverineDryIocMapperlyMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.MediatR, IocContainerKind.Lamar, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Enabled)]
public sealed class FastEndpointsMediatRLamarMapperlyMartenCached : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.Lamar, MapperKind.Mapperly, DataStoreKind.Marten, CacheKind.Disabled)]
public sealed class FastEndpointsWolverineLamarMapperlyMartenNoCache : BenchmarkStartupBase
{
}

[BenchmarkStartup(EndpointStyle.FastEndpoints, MediatorKind.Wolverine, IocContainerKind.Lamar, MapperKind.Mapperly, DataStoreKind.Mongo, CacheKind.Disabled)]
public sealed class FastEndpointsWolverineLamarMapperlyMongoNoCache : BenchmarkStartupBase
{
}
