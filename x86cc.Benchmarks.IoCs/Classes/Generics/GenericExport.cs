namespace x86cc.Benchmarks.IoCs.Classes.Generics
{
    public class GenericExport<T> : IGenericInterface<T>
    {
        public T Value { get; set; } = default!;
    }
}
