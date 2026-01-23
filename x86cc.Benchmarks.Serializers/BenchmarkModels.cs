using MessagePack;
using ProtoBuf;

namespace x86cc.Benchmarks.Serializers;

[MessagePackObject]
[ProtoContract]
public class CustomerOrderRoot
{
    [Key(0)]
    [ProtoMember(1)]
    public Guid Id { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string OrderNumber { get; set; } = string.Empty;
    [Key(2)]
    [ProtoMember(3)]
    public OrderStatus Status { get; set; }
    [Key(3)]
    [ProtoMember(4)]
    public DateTime CreatedAt { get; set; }
    [Key(4)]
    [ProtoMember(5)]
    public DateTime UpdatedAt { get; set; }
    [Key(5)]
    [ProtoMember(6)]
    public Customer Customer { get; set; } = new();
    [Key(6)]
    [ProtoMember(7)]
    public Address ShippingAddress { get; set; } = new();
    [Key(7)]
    [ProtoMember(8)]
    public Address BillingAddress { get; set; } = new();
    [Key(8)]
    [ProtoMember(9)]
    public PaymentDetails Payment { get; set; } = new();
    [Key(9)]
    [ProtoMember(10)]
    public List<OrderLine> Lines { get; set; } = [];
    [Key(10)]
    [ProtoMember(11)]
    public List<Discount> Discounts { get; set; } = [];
    [Key(11)]
    [ProtoMember(12)]
    public AuditInfo Audit { get; set; } = new();
    [Key(12)]
    [ProtoMember(13)]
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

[MessagePackObject]
[ProtoContract]
public class Customer
{
    [Key(0)]
    [ProtoMember(1)]
    public Guid CustomerId { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string FirstName { get; set; } = string.Empty;
    [Key(2)]
    [ProtoMember(3)]
    public string LastName { get; set; } = string.Empty;
    [Key(3)]
    [ProtoMember(4)]
    public string Email { get; set; } = string.Empty;
    [Key(4)]
    [ProtoMember(5)]
    public string? Phone { get; set; }
    [Key(5)]
    [ProtoMember(6)]
    public CustomerPreferences Preferences { get; set; } = new();
    [Key(6)]
    [ProtoMember(7)]
    public List<ContactMethod> Contacts { get; set; } = [];
}

[MessagePackObject]
[ProtoContract]
public class CustomerPreferences
{
    [Key(0)]
    [ProtoMember(1)]
    public bool ReceiveNewsletter { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string? Locale { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public string? TimeZone { get; set; }
}

[MessagePackObject]
[ProtoContract]
public class ContactMethod
{
    [Key(0)]
    [ProtoMember(1)]
    public ContactType Type { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string Value { get; set; } = string.Empty;
    [Key(2)]
    [ProtoMember(3)]
    public bool IsPrimary { get; set; }
}

public enum ContactType
{
    Email,
    Phone,
    Sms
}

[MessagePackObject]
[ProtoContract]
public class Address
{
    [Key(0)]
    [ProtoMember(1)]
    public string Line1 { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string? Line2 { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public string City { get; set; } = string.Empty;
    [Key(3)]
    [ProtoMember(4)]
    public string Region { get; set; } = string.Empty;
    [Key(4)]
    [ProtoMember(5)]
    public string PostalCode { get; set; } = string.Empty;
    [Key(5)]
    [ProtoMember(6)]
    public string CountryCode { get; set; } = string.Empty;
}

[MessagePackObject]
[ProtoContract]
public class PaymentDetails
{
    [Key(0)]
    [ProtoMember(1)]
    public PaymentMethod Method { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string? Provider { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public string? MaskedAccount { get; set; }
    [Key(3)]
    [ProtoMember(4)]
    public Money Total { get; set; } = new();
    [Key(4)]
    [ProtoMember(5)]
    public Money Tax { get; set; } = new();
    [Key(5)]
    [ProtoMember(6)]
    public Money Shipping { get; set; } = new();
    [Key(6)]
    [ProtoMember(7)]
    public Money Discount { get; set; } = new();
}

public enum PaymentMethod
{
    Card,
    BankTransfer,
    Wallet,
    CashOnDelivery
}

[MessagePackObject]
[ProtoContract]
public class Money
{
    [Key(0)]
    [ProtoMember(1)]
    public decimal Amount { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public string Currency { get; set; } = "USD";
}

[MessagePackObject]
[ProtoContract]
public class OrderLine
{
    [Key(0)]
    [ProtoMember(1)]
    public int LineNumber { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public Product Product { get; set; } = new();
    [Key(2)]
    [ProtoMember(3)]
    public int Quantity { get; set; }
    [Key(3)]
    [ProtoMember(4)]
    public Money LineTotal { get; set; } = new();
    [Key(4)]
    [ProtoMember(5)]
    public List<AttributeValue> Attributes { get; set; } = [];
    [Key(5)]
    [ProtoMember(6)]
    public Fulfillment Fulfillment { get; set; } = new();
}

[MessagePackObject]
[ProtoContract]
public class Product
{
    [Key(0)]
    [ProtoMember(1)]
    public string Sku { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string Name { get; set; } = string.Empty;
    [Key(2)]
    [ProtoMember(3)]
    public string? Category { get; set; }
    [Key(3)]
    [ProtoMember(4)]
    public Dimensions Dimensions { get; set; } = new();
    [Key(4)]
    [ProtoMember(5)]
    public decimal WeightKg { get; set; }
}

[MessagePackObject]
[ProtoContract]
public class Dimensions
{
    [Key(0)]
    [ProtoMember(1)]
    public decimal Width { get; set; }
    [Key(1)]
    [ProtoMember(2)]
    public decimal Height { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public decimal Depth { get; set; }
}

[MessagePackObject]
[ProtoContract]
public class AttributeValue
{
    [Key(0)]
    [ProtoMember(1)]
    public string Name { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string Value { get; set; } = string.Empty;
}

[MessagePackObject]
[ProtoContract]
public class Fulfillment
{
    [Key(0)]
    [ProtoMember(1)]
    public string WarehouseCode { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string? Carrier { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public DateTime? EstimatedShipDate { get; set; }
}

[MessagePackObject]
[ProtoContract]
public class Discount
{
    [Key(0)]
    [ProtoMember(1)]
    public string Code { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string? Description { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public Money Amount { get; set; } = new();
}

[MessagePackObject]
[ProtoContract]
public class AuditInfo
{
    [Key(0)]
    [ProtoMember(1)]
    public string CreatedBy { get; set; } = string.Empty;
    [Key(1)]
    [ProtoMember(2)]
    public string? ApprovedBy { get; set; }
    [Key(2)]
    [ProtoMember(3)]
    public DateTime? ApprovedAt { get; set; }
    [Key(3)]
    [ProtoMember(4)]
    public string? Notes { get; set; }
}
