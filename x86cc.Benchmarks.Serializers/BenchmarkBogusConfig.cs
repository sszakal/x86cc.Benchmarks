using Bogus;

namespace x86cc.Benchmarks.Serializers;

public static class BenchmarkBogusConfig
{
    public static Faker<CustomerOrderRoot> CreateCustomerOrderRootFaker(int? seed = null)
    {
        if (seed.HasValue)
        {
            Randomizer.Seed = new Random(seed.Value);
        }

        var dimensionsFaker = new Faker<Dimensions>()
            .RuleFor(d => d.Width, f => f.Random.Decimal(5, 120))
            .RuleFor(d => d.Height, f => f.Random.Decimal(5, 120))
            .RuleFor(d => d.Depth, f => f.Random.Decimal(5, 120));

        var moneyFaker = new Faker<Money>()
            .RuleFor(m => m.Amount, f => f.Random.Decimal(1, 2000))
            .RuleFor(m => m.Currency, _ => "USD");

        var productFaker = new Faker<Product>()
            .RuleFor(p => p.Sku, f => f.Commerce.Ean13())
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Dimensions, _ => dimensionsFaker.Generate())
            .RuleFor(p => p.WeightKg, f => f.Random.Decimal(0.1m, 25m));

        var attributeFaker = new Faker<AttributeValue>()
            .RuleFor(a => a.Name, f => f.Commerce.ProductAdjective())
            .RuleFor(a => a.Value, f => f.Commerce.ProductMaterial());

        var fulfillmentFaker = new Faker<Fulfillment>()
            .RuleFor(f => f.WarehouseCode, f => $"WH-{f.Random.Number(1, 20):D2}")
            .RuleFor(f => f.Carrier, f => f.PickRandom("UPS", "FedEx", "DHL", "USPS"))
            .RuleFor(f => f.EstimatedShipDate, f => f.Date.Soon(10));

        var orderLineFaker = new Faker<OrderLine>()
            .RuleFor(l => l.LineNumber, f => f.IndexFaker + 1)
            .RuleFor(l => l.Product, _ => productFaker.Generate())
            .RuleFor(l => l.Quantity, f => f.Random.Number(1, 5))
            .RuleFor(l => l.LineTotal, _ => moneyFaker.Generate())
            .RuleFor(l => l.Attributes, f => f.Make(f.Random.Number(1, 4), () => attributeFaker.Generate()))
            .RuleFor(l => l.Fulfillment, _ => fulfillmentFaker.Generate());

        var addressFaker = new Faker<Address>()
            .RuleFor(a => a.Line1, f => f.Address.StreetAddress())
            .RuleFor(a => a.Line2, f => f.Random.Bool(0.3f) ? f.Address.SecondaryAddress() : null)
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.Region, f => f.Address.State())
            .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
            .RuleFor(a => a.CountryCode, f => f.Address.CountryCode());

        var preferencesFaker = new Faker<CustomerPreferences>()
            .RuleFor(p => p.ReceiveNewsletter, f => f.Random.Bool())
            .RuleFor(p => p.Locale, f => f.PickRandom("en-US", "en-GB", "de-DE", "fr-FR"))
            .RuleFor(p => p.TimeZone, f => f.PickRandom("UTC", "America/New_York", "Europe/Berlin"));

        var contactFaker = new Faker<ContactMethod>()
            .RuleFor(c => c.Type, f => f.PickRandom<ContactType>())
            .RuleFor(c => c.Value, (f, c) => c.Type switch
            {
                ContactType.Email => f.Internet.Email(),
                ContactType.Phone => f.Phone.PhoneNumber(),
                ContactType.Sms => f.Phone.PhoneNumber(),
                _ => f.Internet.UserName()
            })
            .RuleFor(c => c.IsPrimary, f => f.Random.Bool(0.2f));

        var customerFaker = new Faker<Customer>()
            .RuleFor(c => c.CustomerId, f => f.Random.Guid())
            .RuleFor(c => c.FirstName, f => f.Name.FirstName())
            .RuleFor(c => c.LastName, f => f.Name.LastName())
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email(c.FirstName, c.LastName))
            .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(c => c.Preferences, _ => preferencesFaker.Generate())
            .RuleFor(c => c.Contacts, f => f.Make(f.Random.Number(1, 3), () => contactFaker.Generate()));

        var discountFaker = new Faker<Discount>()
            .RuleFor(d => d.Code, f => $"SAVE-{f.Random.AlphaNumeric(6).ToUpperInvariant()}")
            .RuleFor(d => d.Description, f => f.Commerce.ProductAdjective())
            .RuleFor(d => d.Amount, _ => moneyFaker.Generate());

        var auditFaker = new Faker<AuditInfo>()
            .RuleFor(a => a.CreatedBy, f => f.Internet.UserName())
            .RuleFor(a => a.ApprovedBy, f => f.Random.Bool(0.6f) ? f.Internet.UserName() : null)
            .RuleFor(a => a.ApprovedAt, f => f.Random.Bool(0.6f) ? f.Date.Recent(30) : null)
            .RuleFor(a => a.Notes, f => f.Random.Bool(0.2f) ? f.Lorem.Sentence() : null);

        var paymentFaker = new Faker<PaymentDetails>()
            .RuleFor(p => p.Method, f => f.PickRandom<PaymentMethod>())
            .RuleFor(p => p.Provider, f => f.PickRandom("Stripe", "Adyen", "PayPal", "Square"))
            .RuleFor(p => p.MaskedAccount, f => $"**** **** **** {f.Random.Number(1000, 9999)}")
            .RuleFor(p => p.Total, _ => moneyFaker.Generate())
            .RuleFor(p => p.Tax, _ => moneyFaker.Generate())
            .RuleFor(p => p.Shipping, _ => moneyFaker.Generate())
            .RuleFor(p => p.Discount, _ => moneyFaker.Generate());

        return new Faker<CustomerOrderRoot>()
            .RuleFor(o => o.Id, f => f.Random.Guid())
            .RuleFor(o => o.OrderNumber, f => $"ORD-{f.Random.Number(100000, 999999)}")
            .RuleFor(o => o.Status, f => f.PickRandom<OrderStatus>())
            .RuleFor(o => o.CreatedAt, f => f.Date.Past(1))
            .RuleFor(o => o.UpdatedAt, (f, o) => f.Date.Between(o.CreatedAt, DateTime.UtcNow))
            .RuleFor(o => o.Customer, _ => customerFaker.Generate())
            .RuleFor(o => o.ShippingAddress, _ => addressFaker.Generate())
            .RuleFor(o => o.BillingAddress, _ => addressFaker.Generate())
            .RuleFor(o => o.Payment, _ => paymentFaker.Generate())
            .RuleFor(o => o.Lines, f => f.Make(f.Random.Number(1, 6), () => orderLineFaker.Generate()))
            .RuleFor(o => o.Discounts, f => f.Make(f.Random.Number(0, 2), () => discountFaker.Generate()))
            .RuleFor(o => o.Audit, _ => auditFaker.Generate())
            .RuleFor(o => o.Metadata, f => new Dictionary<string, string?>
            {
                ["channel"] = f.PickRandom("web", "mobile", "pos"),
                ["campaign"] = f.Random.Bool(0.4f) ? f.Commerce.Department() : string.Empty,
                ["priority"] = f.Random.Bool(0.2f) ? "high" : string.Empty
            });
    }
}
