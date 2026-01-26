using MicroResolver;
using x86cc.Benchmarks.IoCs.Classes.Complex;
using x86cc.Benchmarks.IoCs.Classes.Dummy;
using x86cc.Benchmarks.IoCs.Classes.Multiple;
using x86cc.Benchmarks.IoCs.Classes.Properties;
using x86cc.Benchmarks.IoCs.Classes.Standard;

namespace x86cc.Benchmarks.IoCs.Adapters
{
    public sealed class MicroResolverContainerAdapter : ContainerAdapterBase
    {
        private ObjectResolver resolver;

        public override object Resolve(Type type)
        {
            return this.resolver.Resolve(type);
        }

        public override void Dispose()
        {
            // does not support cleanup
        }

        public override void Prepare()
        {
            this.resolver = ObjectResolver.Create();

            RegisterDummies(this.resolver);
            RegisterStandard(this.resolver);
            RegisterComplex(this.resolver);
            RegisterPropertyInjection(this.resolver);
            RegisterMultiple(this.resolver);

            this.resolver.Compile();
        }

        public override void PrepareBasic()
        {
            this.resolver = ObjectResolver.Create();

            RegisterDummies(this.resolver);
            RegisterStandard(this.resolver);
            RegisterComplex(this.resolver);

            this.resolver.Compile();
        }

        private static void RegisterDummies(ObjectResolver resolver)
        {
            resolver.Register<IDummyOne, DummyOne>();
            resolver.Register<IDummyTwo, DummyTwo>();
            resolver.Register<IDummyThree, DummyThree>();
            resolver.Register<IDummyFour, DummyFour>();
            resolver.Register<IDummyFive, DummyFive>();
            resolver.Register<IDummySix, DummySix>();
            resolver.Register<IDummySeven, DummySeven>();
            resolver.Register<IDummyEight, DummyEight>();
            resolver.Register<IDummyNine, DummyNine>();
            resolver.Register<IDummyTen, DummyTen>();
        }

        private static void RegisterStandard(ObjectResolver resolver)
        {
            resolver.Register<ISingleton1, Singleton1>(Lifestyle.Singleton);
            resolver.Register<ISingleton2, Singleton2>(Lifestyle.Singleton);
            resolver.Register<ISingleton3, Singleton3>(Lifestyle.Singleton);
            resolver.Register<ITransient1, Transient1>();
            resolver.Register<ITransient2, Transient2>();
            resolver.Register<ITransient3, Transient3>();
            resolver.Register<ICombined1, Combined1>();
            resolver.Register<ICombined2, Combined2>();
            resolver.Register<ICombined3, Combined3>();
        }

        private static void RegisterComplex(ObjectResolver resolver)
        {
            resolver.Register<IFirstService, FirstService>(Lifestyle.Singleton);
            resolver.Register<ISecondService, SecondService>(Lifestyle.Singleton);
            resolver.Register<IThirdService, ThirdService>(Lifestyle.Singleton);
            resolver.Register<ISubObjectOne, SubObjectOne>();
            resolver.Register<ISubObjectTwo, SubObjectTwo>();
            resolver.Register<ISubObjectThree, SubObjectThree>();
            resolver.Register<IComplex1, Complex1>();
            resolver.Register<IComplex2, Complex2>();
            resolver.Register<IComplex3, Complex3>();
        }

        private static void RegisterPropertyInjection(ObjectResolver resolver)
        {
            resolver.Register<IServiceA, ServiceA>(Lifestyle.Singleton);
            resolver.Register<IServiceB, ServiceB>(Lifestyle.Singleton);
            resolver.Register<IServiceC, ServiceC>(Lifestyle.Singleton);
            resolver.Register<ISubObjectA, SubObjectA>();
            resolver.Register<ISubObjectB, SubObjectB>();
            resolver.Register<ISubObjectC, SubObjectC>();
            resolver.Register<IComplexPropertyObject1, ComplexPropertyObject1>();
            resolver.Register<IComplexPropertyObject2, ComplexPropertyObject2>();
            resolver.Register<IComplexPropertyObject3, ComplexPropertyObject3>();
        }

        private static void RegisterMultiple(ObjectResolver resolver)
        {
            resolver.RegisterCollection<ISimpleAdapter>(new[]
            {
                typeof(SimpleAdapterOne),
                typeof(SimpleAdapterTwo),
                typeof(SimpleAdapterThree),
                typeof(SimpleAdapterFour),
                typeof(SimpleAdapterFive)
            });
            resolver.Register<ImportMultiple1, ImportMultiple1>();
            resolver.Register<ImportMultiple2, ImportMultiple2>();
            resolver.Register<ImportMultiple3, ImportMultiple3>();
        }
    }
}
