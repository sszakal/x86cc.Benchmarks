using Riok.Mapperly.Abstractions;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers.Config;

[Mapper(UseDeepCloning = true)]
public partial class BenchmarkMapperlyMapper
{
    public partial CustomerOrderRootDto Map(CustomerOrderRoot source);

    public partial CustomerOrderRootDto[] Map(CustomerOrderRoot[] source);
    
    [MapperIgnoreSource(nameof(Customer.FirstName))]
    [MapperIgnoreSource(nameof(Customer.LastName))]
    public partial CustomerDto Map(Customer source);
}
