using System.Linq.Expressions;

namespace x86cc.Benchmarks.AspNetCore.Specifications;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }
    int Take { get; }
}
