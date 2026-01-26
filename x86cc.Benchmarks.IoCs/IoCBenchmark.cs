using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using x86cc.Benchmarks.Common;
using x86cc.Benchmarks.IoCs.Adapters;
using x86cc.Benchmarks.IoCs.Classes.AspNet;
using x86cc.Benchmarks.IoCs.Classes.Child;
using x86cc.Benchmarks.IoCs.Classes.Complex;
using x86cc.Benchmarks.IoCs.Classes.Conditional;
using x86cc.Benchmarks.IoCs.Classes.Dummy;
using x86cc.Benchmarks.IoCs.Classes.Generics;
using x86cc.Benchmarks.IoCs.Classes.Multiple;
using x86cc.Benchmarks.IoCs.Classes.Properties;
using x86cc.Benchmarks.IoCs.Classes.Standard;

namespace x86cc.Benchmarks.IoCs;

[BenchmarkCategory("IoC")]
[MemoryDiagnoser]
[GcServer(true)]
[Config(typeof(DefaultBenchmarkConfig))]
public abstract class IoCBenchmark : IDisposable
{
    private IContainerAdapter container;
    
    protected virtual bool SupportsInterception => false;

    protected virtual bool SupportsPropertyInjection => false;

    protected virtual bool SupportsChildContainer => false;

    protected virtual bool SupportAspNetCore => false;

    protected virtual bool SupportsConditional => false;

    protected virtual bool SupportGeneric => false;

    protected virtual bool SupportsMultiple => false;

    protected virtual bool SupportsTransient => true;

    protected virtual bool SupportsCombined => true;

    protected virtual bool SupportsBasic => true;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        container = BuildContainer();
        this.ZeroCounters();
        container.Prepare();
    }

    protected abstract IContainerAdapter BuildContainer();
    
    private int IterationCount = 0;
    
    [IterationSetup]
    public void LoopCount()
    {
        IterationCount++;
    }

    [IterationCleanup(Targets = [nameof(Singleton)])]
    public void SingletonValidation()
    {
        if (Singleton1.Instances > 1 || Singleton2.Instances > 1 || Singleton2.Instances > 1)
        {
            throw new Exception("Singleton instance count must be 1.");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Singleton()
    {
        var singleton1 = (ISingleton1)container.Resolve(typeof(ISingleton1));
        var singleton2 = (ISingleton2)container.Resolve(typeof(ISingleton2));
        var singleton3 = (ISingleton3)container.Resolve(typeof(ISingleton3));
    }
    
    [IterationCleanup(Targets = [nameof(Transient)])]
    public void TransientValidation()
    {
        if (Transient1.Instances != IterationCount
            || Transient2.Instances != IterationCount
            || Transient3.Instances != IterationCount)
        {
            throw new Exception($"Transient count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Transient()
    {
        if (!SupportsTransient) throw new Exception("NA");
        var transient1 = (ITransient1)container.Resolve(typeof(ITransient1));
        var transient2 = (ITransient2)container.Resolve(typeof(ITransient2));
        var transient3 = (ITransient3)container.Resolve(typeof(ITransient3));
    } 
    
    [IterationCleanup(Targets = [nameof(Combined)])]
    public void CombinedValidation()
    {
        if (Combined1.Instances != IterationCount
            || Combined2.Instances != IterationCount
            || Combined3.Instances != IterationCount)
        {
            throw new Exception($"Combined count must be {IterationCount}");
        }

        if (Transient1.Instances != IterationCount
            || Transient2.Instances != IterationCount
            || Transient3.Instances != IterationCount)
        {
            throw new Exception($"Transient count must be {IterationCount}");
        }

        if (Singleton1.Instances > 1 || Singleton2.Instances > 1 || Singleton2.Instances > 1)
        {
            throw new Exception("Singleton instance count must be 1.");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Combined()
    {
        if (!SupportsCombined) throw new Exception("NA");
        var combined1 = (ICombined1)container.Resolve(typeof(ICombined1));
        var combined2 = (ICombined2)container.Resolve(typeof(ICombined2));
        var combined3 = (ICombined3)container.Resolve(typeof(ICombined3));
    }
    
    [IterationCleanup(Targets = [nameof(Complex)])]
    public void ComplexValidation()
    {
        if (Complex1.Instances != IterationCount
            || Complex2.Instances != IterationCount
            || Complex3.Instances != IterationCount)
        {
            throw new Exception($"Complex count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Complex()
    {
        var complex1 = (IComplex1)container.Resolve(typeof(IComplex1));
        var complex2 = (IComplex2)container.Resolve(typeof(IComplex2));
        var complex3 = (IComplex3)container.Resolve(typeof(IComplex3));
    }
    
    [IterationCleanup(Targets = [nameof(Property)])]
    public void PropertyValidation()
    {
        if (ComplexPropertyObject1.Instances != IterationCount
            || ComplexPropertyObject2.Instances != IterationCount
            || ComplexPropertyObject3.Instances != IterationCount)
        {
            throw new Exception($"ComplexPropertyObject count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Property()
    {
        if (!SupportsPropertyInjection) throw new Exception("NA");
        
        var complex1 = (IComplexPropertyObject1)container.Resolve(typeof(IComplexPropertyObject1));
        var complex2 = (IComplexPropertyObject2)container.Resolve(typeof(IComplexPropertyObject2));
        var complex3 = (IComplexPropertyObject3)container.Resolve(typeof(IComplexPropertyObject3));
    } 
    
    [IterationCleanup(Targets = [nameof(Generics)])]
    public void GenericsValidation()
    {
        if (ImportGeneric<int>.Instances != IterationCount
            || ImportGeneric<float>.Instances != IterationCount
            || ImportGeneric<object>.Instances != IterationCount)
        {
            throw new Exception($"ImportGeneric count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Generics()
    {
        if (!SupportGeneric) throw new Exception("NA");
        
        var generic1 = (ImportGeneric<int>)container.Resolve(typeof(ImportGeneric<int>));
        var generic2 = (ImportGeneric<float>)container.Resolve(typeof(ImportGeneric<float>));
        var generic3 = (ImportGeneric<object>)container.Resolve(typeof(ImportGeneric<object>));
    } 
    
    
    [IterationCleanup(Targets = [nameof(IEnumerable)])]
    public void IEnumerableValidation()
    {
        if (ImportMultiple1.Instances != IterationCount
            || ImportMultiple2.Instances != IterationCount
            || ImportMultiple3.Instances != IterationCount)
        {
            throw new Exception($"ImportMultiple count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void IEnumerable()
    {
        if (!SupportsMultiple) throw new Exception("NA");
        
        var importMultiple1 = (ImportMultiple1)container.Resolve(typeof(ImportMultiple1));
        var importMultiple2 = (ImportMultiple2)container.Resolve(typeof(ImportMultiple2));
        var importMultiple3 = (ImportMultiple3)container.Resolve(typeof(ImportMultiple3));
    }
    
    [IterationCleanup(Targets = [nameof(Conditional)])]
    public void ConditionalValidation()
    {
        if (ImportConditionObject1.Instances != IterationCount
            || ImportConditionObject2.Instances != IterationCount
            || ImportConditionObject3.Instances != IterationCount)
        {
            throw new Exception($"ImportConditionObject count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Conditional()
    {
        if (!SupportsConditional) throw new Exception("NA");
        
        var importConditionObject1 = (ImportConditionObject1)container.Resolve(typeof(ImportConditionObject1));
        var importConditionObject2 = (ImportConditionObject2)container.Resolve(typeof(ImportConditionObject2));
        var importConditionObject3 = (ImportConditionObject3)container.Resolve(typeof(ImportConditionObject3));
    }
    
    [IterationCleanup(Targets = [nameof(ChildContainer)])]
    public void ChildContainerValidation()
    {
        if (ScopedCombined1.Instances != IterationCount
            || ScopedCombined2.Instances != IterationCount
            || ScopedCombined3.Instances != IterationCount)
        {
            throw new Exception($"ScopedCombined count must be {this.LoopCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void ChildContainer()
    {
        if (!SupportsChildContainer) throw new Exception("NA");
        
        using (var childContainer = container.CreateChildContainerAdapter())
        {
            childContainer.Prepare();

            var scopedCombined = (ICombined1)childContainer.Resolve(typeof(ICombined1));
        }

        using (var childContainer = container.CreateChildContainerAdapter())
        {
            childContainer.Prepare();

            var scopedCombined = (ICombined2)childContainer.Resolve(typeof(ICombined2));
        }

        using (var childContainer = container.CreateChildContainerAdapter())
        {
            childContainer.Prepare();

            var scopedCombined = (ICombined3)childContainer.Resolve(typeof(ICombined3));
        }
    }
    
    [IterationCleanup(Targets = [nameof(AspNetCore)])]
    public void AspNetCoreValidation()
    {
        if (TestController1.Instances != IterationCount ||
            TestController1.DisposeCount != IterationCount)
        {
            throw new Exception($"TestController1 count must be {IterationCount}");
        }

        if (TestController2.Instances != IterationCount ||
            TestController2.DisposeCount != IterationCount)
        {
            throw new Exception($"TestController2 count must be {IterationCount}");
        }

        if (TestController3.Instances != IterationCount ||
            TestController3.DisposeCount != IterationCount)
        {
            throw new Exception($"TestController3 count must be {IterationCount}");
        }

        if (RepositoryTransient1.Instances != IterationCount * 3 ||
            RepositoryTransient2.Instances != IterationCount * 3 ||
            RepositoryTransient3.Instances != IterationCount * 3 ||
            RepositoryTransient4.Instances != IterationCount * 3 ||
            RepositoryTransient5.Instances != IterationCount * 3)
        {
            throw new Exception($"RepositoryTransient count must be {IterationCount}");
        }

        if (ScopedService1.Instances != IterationCount * 3 ||
            ScopedService2.Instances != IterationCount * 3 ||
            ScopedService3.Instances != IterationCount * 3 ||
            ScopedService4.Instances != IterationCount * 3 ||
            ScopedService5.Instances != IterationCount * 3)
        {
            throw new Exception($"ScopedService count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void AspNetCore()
    {
        if (!SupportAspNetCore) throw new Exception("NA");
        
        var factory = (IServiceScopeFactory)container.Resolve(typeof(IServiceScopeFactory));

        using (var scope = factory.CreateScope())
        {
            var controller = scope.ServiceProvider.GetService(typeof(TestController1));
        }

        factory = (IServiceScopeFactory)container.Resolve(typeof(IServiceScopeFactory));

        using (var scope = factory.CreateScope())
        {
            var controller = scope.ServiceProvider.GetService(typeof(TestController2));
        }

        factory = (IServiceScopeFactory)container.Resolve(typeof(IServiceScopeFactory));

        using (var scope = factory.CreateScope())
        {
            var controller = scope.ServiceProvider.GetService(typeof(TestController3));
        }
    }    
    
    [IterationCleanup(Targets = [nameof(InterceptionWithProxy)])]
    public void InterceptionWithProxyValidation()
    {
        if (Calculator1.Instances != IterationCount
            || Calculator2.Instances != IterationCount
            || Calculator3.Instances != IterationCount)
        {
            throw new Exception($"Calculator count must be {IterationCount}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void InterceptionWithProxy()
    {
        if (!SupportsInterception) throw new Exception("NA");
        
        var result1 = (ICalculator1)container.Resolve(typeof(ICalculator1));
        var result2 = (ICalculator2)container.Resolve(typeof(ICalculator2));
        var result3 = (ICalculator3)container.Resolve(typeof(ICalculator3));

        result1.Add(5, 10);
        result2.Add(5, 10);
        result3.Add(5, 10);
    }    
    
    [IterationCleanup(Targets = [nameof(PrepareAndRegister)])]
    public void PrepareAndRegisterCleanup()
    {
        container.Dispose();
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void PrepareAndRegister()
    {
        if (!SupportsInterception) throw new Exception("NA");
        
        container.PrepareBasic();
        container.Dispose();
    }   
    
    [IterationCleanup(Targets = [nameof(PrepareAndRegisterAndSimpleResolve)])]
    public void PrepareAndRegisterAndSimpleResolveValidation()
    {
        if (Singleton1.Instances != IterationCount)
        {
            throw new Exception($"Singleton1 count must be {IterationCount} but was {Singleton1.Instances}");
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void PrepareAndRegisterAndSimpleResolve()
    {
        container.PrepareBasic();
        container.Resolve(typeof(IDummyOne));
        container.Resolve(typeof(ISingleton1));
        container.Dispose();
    }
    
    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        container?.Dispose();
        container = null;
    }

    private void ZeroCounters()
        {
            ScopedCombined1.Instances = 0;
            ScopedCombined2.Instances = 0;
            ScopedCombined3.Instances = 0;
            Complex1.Instances = 0;
            Complex2.Instances = 0;
            Complex3.Instances = 0;
            ImportConditionObject1.Instances = 0;
            ImportConditionObject2.Instances = 0;
            ImportConditionObject3.Instances = 0;
            ImportGeneric<int>.Instances = 0;
            ImportGeneric<float>.Instances = 0;
            ImportGeneric<object>.Instances = 0;
            ImportMultiple1.Instances = 0;
            ImportMultiple2.Instances = 0;
            ImportMultiple3.Instances = 0;
            ComplexPropertyObject1.Instances = 0;
            ComplexPropertyObject2.Instances = 0;
            ComplexPropertyObject3.Instances = 0;
            Calculator1.Instances = 0;
            Calculator2.Instances = 0;
            Calculator3.Instances = 0;
            Combined1.Instances = 0;
            Combined2.Instances = 0;
            Combined3.Instances = 0;
            Singleton1.Instances = 0;
            Singleton2.Instances = 0;
            Singleton3.Instances = 0;
            Transient1.Instances = 0;
            Transient2.Instances = 0;
            Transient3.Instances = 0;

            TestController1.DisposeCount = 0;
            TestController1.Instances = 0;
            TestController2.DisposeCount = 0;
            TestController2.Instances = 0;
            TestController3.DisposeCount = 0;
            TestController3.Instances = 0;
            ScopedService1.Instances = 0;
            ScopedService2.Instances = 0;
            ScopedService3.Instances = 0;
            ScopedService4.Instances = 0;
            ScopedService5.Instances = 0;
            RepositoryTransient1.Instances = 0;
            RepositoryTransient2.Instances = 0;
            RepositoryTransient3.Instances = 0;
            RepositoryTransient4.Instances = 0;
            RepositoryTransient5.Instances = 0;
        }

    public void Dispose()
    {
        container?.Dispose();
    }
}