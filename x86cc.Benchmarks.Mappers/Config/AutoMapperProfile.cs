using System.Collections.Generic;
using AutoMapper;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers.Config;

public class BenchmarkProfile : Profile
{
    public BenchmarkProfile()
    {
        CreateMap<CustomerOrderRoot, CustomerOrderRootDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => new Dictionary<string, string?>(src.Metadata)));

        CreateMap<Customer, CustomerDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        CreateMap<ContactMethod, ContactMethodDto>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

        CreateMap<PaymentMethod, string>().ConvertUsing(src => src.ToString());
        CreateMap<OrderStatus, string>().ConvertUsing(src => src.ToString());

        CreateMap<Address, AddressDto>();
        CreateMap<PaymentDetails, PaymentDetailsDto>();
        CreateMap<Money, MoneyDto>();
        CreateMap<OrderLine, OrderLineDto>();
        CreateMap<Product, ProductDto>();
        CreateMap<Dimensions, DimensionsDto>();
        CreateMap<AttributeValue, AttributeValueDto>();
        CreateMap<Fulfillment, FulfillmentDto>();
        CreateMap<Discount, DiscountDto>();
        CreateMap<AuditInfo, AuditInfoDto>();
        CreateMap<CustomerPreferences, CustomerPreferencesDto>();
    }
}
