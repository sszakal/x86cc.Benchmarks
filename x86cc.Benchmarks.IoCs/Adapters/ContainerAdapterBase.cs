using Microsoft.Extensions.DependencyInjection;
using x86cc.Benchmarks.IoCs.Classes.AspNet;

namespace x86cc.Benchmarks.IoCs.Adapters
{
    public abstract class ContainerAdapterBase : IContainerAdapter
    {
        public abstract void PrepareBasic();
        
        public virtual void Prepare()
        {
            this.PrepareBasic(); // by default any prepare should at least support basic one
        }

        public abstract object Resolve(Type type);

        public virtual IChildContainerAdapter CreateChildContainerAdapter()
        {
            throw new NotImplementedException();
        }

        public abstract void Dispose();

        protected void RegisterAspNetCoreClasses(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<TestController1>();
            serviceCollection.AddTransient<TestController2>();
            serviceCollection.AddTransient<TestController3>();
            serviceCollection.AddTransient<IRepositoryTransient1, RepositoryTransient1>();
            serviceCollection.AddTransient<IRepositoryTransient2, RepositoryTransient2>();
            serviceCollection.AddTransient<IRepositoryTransient3, RepositoryTransient3>();
            serviceCollection.AddTransient<IRepositoryTransient4, RepositoryTransient4>();
            serviceCollection.AddTransient<IRepositoryTransient5, RepositoryTransient5>();
            serviceCollection.AddScoped<IScopedService1, ScopedService1>();
            serviceCollection.AddScoped<IScopedService2, ScopedService2>();
            serviceCollection.AddScoped<IScopedService3, ScopedService3>();
            serviceCollection.AddScoped<IScopedService4, ScopedService4>();
            serviceCollection.AddScoped<IScopedService5, ScopedService5>();
        }
    }
}