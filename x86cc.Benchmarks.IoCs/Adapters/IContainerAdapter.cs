namespace x86cc.Benchmarks.IoCs.Adapters
{
    public interface IContainerAdapter : IDisposable
    {
        /// <summary>
        /// Prepares basic registration. All containers support basic features to be named containers.
        /// Allows fair comparison of feature poor vs rich containers, so additional registrations do not degrade richer containers.
        /// </summary>
        void PrepareBasic();

        void Prepare();

        object Resolve(Type type);

        IChildContainerAdapter CreateChildContainerAdapter();
    }
}