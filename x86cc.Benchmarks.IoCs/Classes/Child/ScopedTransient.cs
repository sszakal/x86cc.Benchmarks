using x86cc.Benchmarks.IoCs.Classes.Standard;

namespace x86cc.Benchmarks.IoCs.Classes.Child
{
    public class ScopedTransient : ITransient1
    {
        public void DoSomething()
        {
            Console.WriteLine("ScopedTransient");
        }
    }
}
