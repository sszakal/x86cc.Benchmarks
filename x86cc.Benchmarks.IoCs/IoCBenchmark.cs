using BenchmarkDotNet.Attributes;
using x86cc.Benchmarks.Common;
using x86cc.Benchmarks.IoCs.Adapters;
using x86cc.Benchmarks.IoCs.Classes.Complex;
using x86cc.Benchmarks.IoCs.Classes.Generics;
using x86cc.Benchmarks.IoCs.Classes.Multiple;
using x86cc.Benchmarks.IoCs.Classes.Properties;
using x86cc.Benchmarks.IoCs.Classes.Standard;

namespace x86cc.Benchmarks.IoCs;

[BenchmarkCategory("Caching Systems")]
[MemoryDiagnoser]
[GcServer(true)]
[Config(typeof(DefaultBenchmarkConfig))]
public abstract class IoCBenchmark : IDisposable
{
    protected IContainerAdapter container;

    [GlobalSetup]
    public async Task GlobalSetup()
    {
        container = BuildContainer();
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
            throw new Exception("Singleton instance count must be 1. Container: " + container.Name);
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
            throw new Exception("Singleton instance count must be 1. Container: " + container.Name);
        }
    }
    
    [Benchmark]
    [WarmupCount(10)]
    [IterationCount(500)]
    public void Combined()
    {
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
        var importMultiple1 = (ImportMultiple1)container.Resolve(typeof(ImportMultiple1));
        var importMultiple2 = (ImportMultiple2)container.Resolve(typeof(ImportMultiple2));
        var importMultiple3 = (ImportMultiple3)container.Resolve(typeof(ImportMultiple3));
    }
    
    [GlobalCleanup]
    public async Task GlobalCleanup()
    {
        container?.Dispose();
        container = null;
    }

    public void Dispose()
    {
        container?.Dispose();
    }
}
