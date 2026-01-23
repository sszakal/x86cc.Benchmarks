using StashBoxAttr = Stashbox.Attributes;

namespace x86cc.Benchmarks.IoCs.Classes.Properties
{
    public interface ISubObjectA
    {
        void Verify(string containerName);
    }
    
    public class SubObjectA : ISubObjectA
    {
        [StashBoxAttr.Dependency]
        public IServiceA ServiceA { get; set; }

        public void Verify(string containerName)
        {
            if (this.ServiceA == null)
            {
                throw new Exception("ServiceA was null for SubObjectC for container " + containerName);
            }
        }
    }
}
