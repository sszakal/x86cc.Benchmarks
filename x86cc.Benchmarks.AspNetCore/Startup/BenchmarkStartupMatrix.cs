namespace x86cc.Benchmarks.AspNetCore.Startup;
public static class BenchmarkStartupMatrix
{
    public static readonly Type[] StartupTypes =
    [
        typeof(ControllersWolverineLamarMapperlyMartenCached),
        typeof(FastEndpointsWolverineLamarMapperlyMartenCached),
        typeof(FastEndpointsWolverineLamarMapsterMartenCached),
        typeof(FastEndpointsWolverineDefaultMapperlyMartenCached),
        typeof(FastEndpointsWolverineDryIocMapperlyMartenCached),
        typeof(FastEndpointsMediatRLamarMapperlyMartenCached),
        typeof(FastEndpointsWolverineLamarMapperlyMartenNoCache),
        typeof(FastEndpointsWolverineLamarMapperlyMongoNoCache),
    ];
}
