using System.Collections.Generic;
using Mapster;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers.Config;

public static class MapsterMappings
{
    public static TypeAdapterConfig CreateConfig()
    {
        var config = new TypeAdapterConfig();

        config.NewConfig<CustomerOrderRoot, CustomerOrderRootDto>()
            .Map(dest => dest.Metadata, src => new Dictionary<string, string?>(src.Metadata));

        config.NewConfig<Customer, CustomerDto>()
            .Map(dest => dest.FullName, src => src.FullName);

        config.NewConfig<ContactMethod, ContactMethodDto>()
            .Map(dest => dest.Type, src => src.Type.ToString());

        config.NewConfig<OrderStatus, string>().MapWith(src => src.ToString());
        config.NewConfig<PaymentMethod, string>().MapWith(src => src.ToString());

        return config;
    }
}
