using System.Diagnostics;
using AopAlliance.Intercept;

namespace x86cc.Benchmarks.IoCs.Interception
{
    [Serializable]
    public class SpringInterceptionLogger : IMethodInterceptor
    {
        public object Invoke(IMethodInvocation invocation)
        {
            // Perform logging here, e.g.:
            var args = string.Join(", ", invocation.Arguments.Select(x => (x ?? string.Empty).ToString()));
            Debug.WriteLine($"Spring.NET: {invocation.Method.Name}({args})");

            return invocation.Proceed();
        }
    }
}
