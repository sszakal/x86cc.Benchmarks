namespace x86cc.Benchmarks.Mappers.Models;

public class CustomerOrderRoot
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public Customer Customer { get; set; } = new();
    public Address ShippingAddress { get; set; } = new();
    public Address BillingAddress { get; set; } = new();
    public PaymentDetails Payment { get; set; } = new();
    public List<OrderLine> Lines { get; set; } = [];
    public List<Discount> Discounts { get; set; } = [];
    public AuditInfo Audit { get; set; } = new();
    public Dictionary<string, string?> Metadata { get; set; } = new();
}

public enum OrderStatus
{
    Draft,
    Submitted,
    Paid,
    Shipped,
    Completed,
    Cancelled
}

public class Customer
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public CustomerPreferences Preferences { get; set; } = new();
    public List<ContactMethod> Contacts { get; set; } = [];
}

public class CustomerPreferences
{
    public bool ReceiveNewsletter { get; set; }
    public string? Locale { get; set; }
    public string? TimeZone { get; set; }
}

public class ContactMethod
{
    public ContactType Type { get; set; }
    public string Value { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
}

public enum ContactType
{
    Email,
    Phone,
    Sms
}

public class Address
{
    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
}

public class PaymentDetails
{
    public PaymentMethod Method { get; set; }
    public string? Provider { get; set; }
    public string? MaskedAccount { get; set; }
    public Money Total { get; set; } = new();
    public Money Tax { get; set; } = new();
    public Money Shipping { get; set; } = new();
    public Money Discount { get; set; } = new();
}

public enum PaymentMethod
{
    Card,
    BankTransfer,
    Wallet,
    CashOnDelivery
}

public class Money
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

public class OrderLine
{
    public int LineNumber { get; set; }
    public Product Product { get; set; } = new();
    public int Quantity { get; set; }
    public Money LineTotal { get; set; } = new();
    public List<AttributeValue> Attributes { get; set; } = [];
    public Fulfillment Fulfillment { get; set; } = new();
}

public class Product
{
    public string Sku { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Category { get; set; }
    public Dimensions Dimensions { get; set; } = new();
    public decimal WeightKg { get; set; }
}

public class Dimensions
{
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
}

public class AttributeValue
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class Fulfillment
{
    public string WarehouseCode { get; set; } = string.Empty;
    public string? Carrier { get; set; }
    public DateTimeOffset? EstimatedShipDate { get; set; }
}

public class Discount
{
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Money Amount { get; set; } = new();
}

public class AuditInfo
{
    public string CreatedBy { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTimeOffset? ApprovedAt { get; set; }
    public string? Notes { get; set; }
}
