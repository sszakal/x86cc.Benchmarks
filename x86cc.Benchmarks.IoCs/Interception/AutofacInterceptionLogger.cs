using System.Diagnostics;
using Castle.DynamicProxy;

namespace x86cc.Benchmarks.IoCs.Interception
{
    public class AutofacInterceptionLogger : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            // Perform logging here, e.g.:
            var args = string.Join(", ", invocation.Arguments.Select(x => (x ?? string.Empty).ToString()));
            Debug.WriteLine($"Autofac: {invocation.Method.Name}({args})");

            invocation.Proceed();
        }
    }
}
