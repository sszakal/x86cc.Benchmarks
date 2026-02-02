using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using x86cc.Benchmarks.AspNetCore.Startup;
using DryIoc.Microsoft.DependencyInjection;
using Lamar.Microsoft.DependencyInjection;

namespace x86cc.Benchmarks.AspNetCore;

public sealed class BenchmarkWebApplicationFactory : WebApplicationFactory<BenchmarkAppMarker>
{
    private readonly Type _startupType;

    public BenchmarkWebApplicationFactory(Type startupType)
    {
        _startupType = startupType;
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        var options = BenchmarkStartupAttribute.GetOptions(_startupType);

        var builder = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.None);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup(_startupType);
                webBuilder.UseTestServer();
            });

        if (options.Ioc == IocContainerKind.Lamar)
        {
            builder.UseLamar();
        }
        else if (options.Ioc == IocContainerKind.DryIoc)
        {
            builder.UseServiceProviderFactory(new DryIocServiceProviderFactory());
        }

        return builder;
    }
}
