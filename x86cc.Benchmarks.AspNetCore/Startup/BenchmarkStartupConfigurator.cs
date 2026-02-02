using FastEndpoints;
using JasperFx;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;
using Wolverine;
using x86cc.Benchmarks.AspNetCore.Cache;
using x86cc.Benchmarks.AspNetCore.Containers;
using x86cc.Benchmarks.AspNetCore.Domain;
using x86cc.Benchmarks.AspNetCore.Handlers;
using x86cc.Benchmarks.AspNetCore.Handlers.MediatR;
using x86cc.Benchmarks.AspNetCore.Handlers.Wolverine;
using x86cc.Benchmarks.AspNetCore.Mappers;
using x86cc.Benchmarks.AspNetCore.Models;
using x86cc.Benchmarks.AspNetCore.Repositories;
using x86cc.Benchmarks.AspNetCore.Services;

namespace x86cc.Benchmarks.AspNetCore.Startup;

public static class BenchmarkStartupConfigurator
{
    public static void ConfigureServices(IServiceCollection services, BenchmarkStartupOptions options)
    {
        services.AddRouting();

        if (options.Endpoint == EndpointStyle.Controllers)
        {
            services.AddControllers();
        }
        else
        {
            services.AddFastEndpoints();
        }

        ConfigureMediator(services, options.Mediator);
        ConfigureCache(services, options.Cache);
        ConfigureMapper(services, options.Mapper);
        ConfigureDataStore(services, options.DataStore);
        ConfigureBlogPostService(services, options.Cache);
    }

    public static void ConfigureApp(IApplicationBuilder app, BenchmarkStartupOptions options)
    {
        app.UseRouting();

        if (options.Endpoint == EndpointStyle.Controllers)
        {
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
        else
        {
            app.UseEndpoints(endpoints => endpoints.MapFastEndpoints());
        }
    }

    private static void ConfigureMediator(IServiceCollection services, MediatorKind mediator)
    {
        if (mediator == MediatorKind.MediatR)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<CreateBlogPostHandler>());
            services.AddScoped<IDispatcher, MediatRDispatcher>();
            return;
        }

        services.AddWolverine(options =>
        {
            options.Discovery.IncludeAssembly(typeof(BlogPostHandlers).Assembly);
        });
        services.AddScoped<IDispatcher, WolverineDispatcher>();
    }

    private static void ConfigureCache(IServiceCollection services, CacheKind cache)
    {
        if (cache == CacheKind.Disabled)
        {
            return;
        }

        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(BenchmarkContainers.ValkeyConnectionString));
        services.AddSingleton<ICache<BlogPost>, ValKeyCache<BlogPost>>();
    }

    private static void ConfigureMapper(IServiceCollection services, MapperKind mapper)
    {
        if (mapper == MapperKind.Mapster)
        {
            services.AddSingleton<IBlogPostMapper, MapsterBlogPostMapper>();
            return;
        }

        services.AddSingleton<BlogPostMapperlyMapper>();
        services.AddSingleton<IBlogPostMapper, MapperlyBlogPostMapper>();
    }

    private static void ConfigureDataStore(IServiceCollection services, DataStoreKind dataStore)
    {
        if (dataStore == DataStoreKind.Marten)
        {
            services.AddSingleton<IDocumentStore>(_ =>
            {
                var store = DocumentStore.For(options =>
                {
                    options.Connection(BenchmarkContainers.PostgresConnectionString);
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                    options.Schema.For<BlogPost>().Identity(x => x.Id);
                });
                return store;
            });
            services.AddHostedService<MartenSchemaInitializer>();
            services.AddScoped<IBlogPostRepository, MartenBlogPostRepository>();
            return;
        }

        services.AddSingleton<IMongoClient>(_ => new MongoClient(BenchmarkContainers.MongoConnectionString));
        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase("benchmarks");
        });
        services.AddSingleton<IMongoCollection<BlogPost>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<BlogPost>("blogposts");
        });
        services.AddScoped<IBlogPostRepository, MongoBlogPostRepository>();
    }

    private static void ConfigureBlogPostService(IServiceCollection services, CacheKind cache)
    {
        if (cache == CacheKind.Enabled)
        {
            services.AddScoped<IBlogPostService, CachedBlogPostService>();
            return;
        }

        services.AddScoped<IBlogPostService, BlogPostService>();
    }
}
