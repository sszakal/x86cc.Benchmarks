namespace x86cc.Benchmarks.Mappers.Models;

public class CustomerOrderRootDto
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public CustomerDto Customer { get; set; } = new();
    public AddressDto ShippingAddress { get; set; } = new();
    public AddressDto BillingAddress { get; set; } = new();
    public PaymentDetailsDto Payment { get; set; } = new();
    public List<OrderLineDto> Lines { get; set; } = [];
    public List<DiscountDto> Discounts { get; set; } = [];
    public AuditInfoDto Audit { get; set; } = new();
    public Dictionary<string, string?> Metadata { get; set; } = new();
}

public class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public CustomerPreferencesDto Preferences { get; set; } = new();
    public List<ContactMethodDto> Contacts { get; set; } = [];
}

public class CustomerPreferencesDto
{
    public bool ReceiveNewsletter { get; set; }
    public string? Locale { get; set; }
    public string? TimeZone { get; set; }
}

public class ContactMethodDto
{
    public string Type { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public class AddressDto
{
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}

public class PaymentDetailsDto
{
    public string Method { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public string? MaskedAccount { get; set; }
    public MoneyDto Total { get; set; } = new();
    public MoneyDto Tax { get; set; } = new();
    public MoneyDto Shipping { get; set; } = new();
    public MoneyDto Discount { get; set; } = new();
}

public class MoneyDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

public class OrderLineDto
{
    public int LineNumber { get; set; }
    public ProductDto Product { get; set; } = new();
    public int Quantity { get; set; }
    public MoneyDto LineTotal { get; set; } = new();
    public List<AttributeValueDto> Attributes { get; set; } = [];
    public FulfillmentDto Fulfillment { get; set; } = new();
}

public class ProductDto
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public DimensionsDto Dimensions { get; set; } = new();
    public decimal WeightKg { get; set; }
}

public class DimensionsDto
{
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
}

public class AttributeValueDto
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class FulfillmentDto
{
    public string WarehouseCode { get; set; } = string.Empty;
    public string? Carrier { get; set; }
    public DateTimeOffset? EstimatedShipDate { get; set; }
}

public class DiscountDto
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public MoneyDto Amount { get; set; } = new();
}

public class AuditInfoDto
{
    public string CreatedBy { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public string? Notes { get; set; }
}
