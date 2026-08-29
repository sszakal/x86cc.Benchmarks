namespace x86cc.Benchmarks.AspNetCore.Startup;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class BenchmarkStartupAttribute : Attribute
{
    public BenchmarkStartupAttribute(
        EndpointStyle endpoint,
        MediatorKind mediator,
        IocContainerKind ioc,
        MapperKind mapper,
        DataStoreKind dataStore,
        CacheKind cache)
    {
        Endpoint = endpoint;
        Mediator = mediator;
        Ioc = ioc;
        Mapper = mapper;
        DataStore = dataStore;
        Cache = cache;
    }

    public EndpointStyle Endpoint { get; }
    public MediatorKind Mediator { get; }
    public IocContainerKind Ioc { get; }
    public MapperKind Mapper { get; }
    public DataStoreKind DataStore { get; }
    public CacheKind Cache { get; }

    public BenchmarkStartupOptions ToOptions() =>
        new(Endpoint, Mediator, Ioc, Mapper, DataStore, Cache);

    public static BenchmarkStartupOptions GetOptions(Type startupType)
    {
        var attribute = (BenchmarkStartupAttribute?)Attribute.GetCustomAttribute(startupType, typeof(BenchmarkStartupAttribute));
        if (attribute is null)
        {
            throw new InvalidOperationException($"Missing {nameof(BenchmarkStartupAttribute)} on {startupType.Name}.");
        }

        return attribute.ToOptions();
    }
}
