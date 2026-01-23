using System.Collections.Generic;
using x86cc.Benchmarks.Mappers.Models;

namespace x86cc.Benchmarks.Mappers.Config;

public static class NoMapper
{
    public static CustomerOrderRootDto Map(CustomerOrderRoot source)
    {
        var lines = new List<OrderLineDto>(source.Lines.Count);
        foreach (var line in source.Lines)
        {
            lines.Add(Map(line));
        }

        var discounts = new List<DiscountDto>(source.Discounts.Count);
        foreach (var discount in source.Discounts)
        {
            discounts.Add(Map(discount));
        }

        return new CustomerOrderRootDto
        {
            Id = source.Id,
            OrderNumber = source.OrderNumber,
            Status = source.Status.ToString(),
            CreatedAt = source.CreatedAt,
            UpdatedAt = source.UpdatedAt,
            Customer = Map(source.Customer),
            ShippingAddress = Map(source.ShippingAddress),
            BillingAddress = Map(source.BillingAddress),
            Payment = Map(source.Payment),
            Lines = lines,
            Discounts = discounts,
            Audit = Map(source.Audit),
            Metadata = new Dictionary<string, string?>(source.Metadata)
        };
    }

    public static CustomerOrderRootDto[] Map(CustomerOrderRoot[] source)
    {
        var result = new CustomerOrderRootDto[source.Length];
        for (var i = 0; i < source.Length; i++)
        {
            result[i] = Map(source[i]);
        }

        return result;
    }

    private static CustomerDto Map(Customer source)
    {
        var contacts = new List<ContactMethodDto>(source.Contacts.Count);
        foreach (var contact in source.Contacts)
        {
            contacts.Add(Map(contact));
        }

        return new CustomerDto
        {
            CustomerId = source.CustomerId,
            FullName = source.FullName,
            Email = source.Email,
            Phone = source.Phone,
            Preferences = Map(source.Preferences),
            Contacts = contacts
        };
    }

    private static CustomerPreferencesDto Map(CustomerPreferences source)
    {
        return new CustomerPreferencesDto
        {
            ReceiveNewsletter = source.ReceiveNewsletter,
            Locale = source.Locale,
            TimeZone = source.TimeZone
        };
    }

    private static ContactMethodDto Map(ContactMethod source)
    {
        return new ContactMethodDto
        {
            Type = source.Type.ToString(),
            Value = source.Value,
            IsPrimary = source.IsPrimary
        };
    }

    private static AddressDto Map(Address source)
    {
        return new AddressDto
        {
            Line1 = source.Line1,
            Line2 = source.Line2,
            City = source.City,
            Region = source.Region,
            PostalCode = source.PostalCode,
            CountryCode = source.CountryCode
        };
    }

    private static PaymentDetailsDto Map(PaymentDetails source)
    {
        return new PaymentDetailsDto
        {
            Method = source.Method.ToString(),
            Provider = source.Provider,
            MaskedAccount = source.MaskedAccount,
            Total = Map(source.Total),
            Tax = Map(source.Tax),
            Shipping = Map(source.Shipping),
            Discount = Map(source.Discount)
        };
    }

    private static MoneyDto Map(Money source)
    {
        return new MoneyDto
        {
            Amount = source.Amount,
            Currency = source.Currency
        };
    }

    private static OrderLineDto Map(OrderLine source)
    {
        var attributes = new List<AttributeValueDto>(source.Attributes.Count);
        foreach (var attribute in source.Attributes)
        {
            attributes.Add(Map(attribute));
        }

        return new OrderLineDto
        {
            LineNumber = source.LineNumber,
            Product = Map(source.Product),
            Quantity = source.Quantity,
            LineTotal = Map(source.LineTotal),
            Attributes = attributes,
            Fulfillment = Map(source.Fulfillment)
        };
    }

    private static ProductDto Map(Product source)
    {
        return new ProductDto
        {
            Sku = source.Sku,
            Name = source.Name,
            Category = source.Category,
            Dimensions = Map(source.Dimensions),
            WeightKg = source.WeightKg
        };
    }

    private static DimensionsDto Map(Dimensions source)
    {
        return new DimensionsDto
        {
            Width = source.Width,
            Height = source.Height,
            Depth = source.Depth
        };
    }

    private static AttributeValueDto Map(AttributeValue source)
    {
        return new AttributeValueDto
        {
            Name = source.Name,
            Value = source.Value
        };
    }

    private static FulfillmentDto Map(Fulfillment source)
    {
        return new FulfillmentDto
        {
            WarehouseCode = source.WarehouseCode,
            Carrier = source.Carrier,
            EstimatedShipDate = source.EstimatedShipDate
        };
    }

    private static DiscountDto Map(Discount source)
    {
        return new DiscountDto
        {
            Code = source.Code,
            Description = source.Description,
            Amount = Map(source.Amount)
        };
    }

    private static AuditInfoDto Map(AuditInfo source)
    {
        return new AuditInfoDto
        {
            CreatedBy = source.CreatedBy,
            ApprovedBy = source.ApprovedBy,
            ApprovedAt = source.ApprovedAt,
            Notes = source.Notes
        };
    }
}
