using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace x86cc.Benchmarks.AspNetCore.Startup;

public abstract class BenchmarkStartupBase
{
    private BenchmarkStartupOptions? _options;

    protected BenchmarkStartupOptions Options => _options ??= BenchmarkStartupAttribute.GetOptions(GetType());

    public void ConfigureServices(IServiceCollection services)
    {
        BenchmarkStartupConfigurator.ConfigureServices(services, Options);
    }

    public void Configure(IApplicationBuilder app)
    {
        BenchmarkStartupConfigurator.ConfigureApp(app, Options);
    }
}
