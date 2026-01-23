using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace x86cc.Benchmarks.DBs;

public class CustomerOrderDbContext : DbContext
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    public CustomerOrderDbContext(DbContextOptions<CustomerOrderDbContext> options)
        : base(options)
    {
    }

    public DbSet<CustomerOrderRoot> Orders => Set<CustomerOrderRoot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var metadataConverter = new ValueConverter<Dictionary<string, string?>, string>(
            value => JsonSerializer.Serialize(value, JsonOptions),
            value => JsonSerializer.Deserialize<Dictionary<string, string?>>(value, JsonOptions) ?? new Dictionary<string, string?>());

        var metadataComparer = new ValueComparer<Dictionary<string, string?>>(
            (left, right) => DictionaryEquals(left, right),
            value => DictionaryHashCode(value),
            value => value.ToDictionary(kv => kv.Key, kv => kv.Value));

        modelBuilder.Entity<CustomerOrderRoot>(order =>
        {
            order.ToTable("CustomerOrders");
            order.HasKey(o => o.Id);
            order.Property(o => o.OrderNumber).HasMaxLength(32);
            order.Property(o => o.Status).HasConversion<string>().HasMaxLength(16);
            order.Property(o => o.Metadata).HasConversion(metadataConverter).Metadata.SetValueComparer(metadataComparer);
            order.HasIndex(o => o.Status);
            order.HasIndex(o => o.CreatedAt);
            order.Navigation(o => o.Customer).AutoInclude();
            order.Navigation(o => o.Lines).AutoInclude();
            order.Navigation(o => o.Discounts).AutoInclude();
            order.Navigation(o => o.Payment).AutoInclude();

            order.OwnsOne(o => o.Customer, customer =>
            {
                customer.Property(c => c.CustomerId).HasColumnName("CustomerId");
                customer.Property(c => c.FirstName).HasMaxLength(64);
                customer.Property(c => c.LastName).HasMaxLength(64);
                customer.Property(c => c.Email).HasMaxLength(128);
                customer.Property(c => c.Phone).HasMaxLength(32);

                customer.OwnsOne(c => c.Preferences, preferences =>
                {
                    preferences.Property(p => p.Locale).HasMaxLength(16);
                    preferences.Property(p => p.TimeZone).HasMaxLength(64);
                });

                customer.OwnsMany(c => c.Contacts, contact =>
                {
                    contact.ToTable("CustomerContacts");
                    contact.WithOwner().HasForeignKey("OrderId");
                    contact.Property(c => c.Id);
                    contact.HasKey(c => c.Id);
                    contact.Property(c => c.Type).HasConversion<string>().HasMaxLength(16);
                    contact.Property(c => c.Value).HasMaxLength(128);
                });

                customer.Navigation(c => c.Preferences).AutoInclude();
                customer.Navigation(c => c.Contacts).AutoInclude();
            });

            order.OwnsOne(o => o.ShippingAddress, address =>
            {
                address.Property(a => a.Line1).HasMaxLength(128);
                address.Property(a => a.Line2).HasMaxLength(128);
                address.Property(a => a.City).HasMaxLength(64);
                address.Property(a => a.Region).HasMaxLength(64);
                address.Property(a => a.PostalCode).HasMaxLength(16);
                address.Property(a => a.CountryCode).HasMaxLength(8);
            });

            order.OwnsOne(o => o.BillingAddress, address =>
            {
                address.Property(a => a.Line1).HasMaxLength(128);
                address.Property(a => a.Line2).HasMaxLength(128);
                address.Property(a => a.City).HasMaxLength(64);
                address.Property(a => a.Region).HasMaxLength(64);
                address.Property(a => a.PostalCode).HasMaxLength(16);
                address.Property(a => a.CountryCode).HasMaxLength(8);
            });

            order.OwnsOne(o => o.Payment, payment =>
            {
                payment.Property(p => p.Method).HasConversion<string>().HasMaxLength(16);
                payment.Property(p => p.Provider).HasMaxLength(64);
                payment.Property(p => p.MaskedAccount).HasMaxLength(32);

                payment.OwnsOne(p => p.Total, money => ConfigureMoney(money, "Total"));
                payment.OwnsOne(p => p.Tax, money => ConfigureMoney(money, "Tax"));
                payment.OwnsOne(p => p.Shipping, money => ConfigureMoney(money, "Shipping"));
                payment.OwnsOne(p => p.Discount, money => ConfigureMoney(money, "Discount"));
            });

            order.OwnsMany(o => o.Lines, line =>
            {
                line.ToTable("OrderLines");
                line.WithOwner().HasForeignKey("OrderId");
                line.Property(l => l.Id);
                line.HasKey(l => l.Id);

                line.OwnsOne(l => l.Product, product =>
                {
                    product.Property(p => p.Sku).HasMaxLength(64);
                    product.Property(p => p.Name).HasMaxLength(128);
                    product.Property(p => p.Category).HasMaxLength(64);

                    product.OwnsOne(p => p.Dimensions, dimensions =>
                    {
                        dimensions.Property(d => d.Width).HasPrecision(9, 2);
                        dimensions.Property(d => d.Height).HasPrecision(9, 2);
                        dimensions.Property(d => d.Depth).HasPrecision(9, 2);
                    });
                });

                line.OwnsOne(l => l.LineTotal, money => ConfigureMoney(money, "LineTotal"));

                line.OwnsMany(l => l.Attributes, attribute =>
                {
                    attribute.ToTable("OrderLineAttributes");
                    attribute.WithOwner().HasForeignKey("OrderLineId");
                    attribute.Property(a => a.Id);
                    attribute.HasKey(a => a.Id);
                    attribute.Property(a => a.Name).HasMaxLength(64);
                    attribute.Property(a => a.Value).HasMaxLength(64);
                });

                line.OwnsOne(l => l.Fulfillment, fulfillment =>
                {
                    fulfillment.Property(f => f.WarehouseCode).HasMaxLength(16);
                    fulfillment.Property(f => f.Carrier).HasMaxLength(32);
                });

                line.Navigation(l => l.Attributes).AutoInclude();
                line.Navigation(l => l.Product).AutoInclude();
                line.Navigation(l => l.Fulfillment).AutoInclude();
            });

            order.OwnsMany(o => o.Discounts, discount =>
            {
                discount.ToTable("OrderDiscounts");
                discount.WithOwner().HasForeignKey("OrderId");
                discount.Property(d => d.Id);
                discount.HasKey(d => d.Id);
                discount.Property(d => d.Code).HasMaxLength(32);
                discount.Property(d => d.Description).HasMaxLength(128);
                discount.OwnsOne(d => d.Amount, money => ConfigureMoney(money, "Amount"));
            });

            order.OwnsOne(o => o.Audit, audit =>
            {
                audit.Property(a => a.CreatedBy).HasMaxLength(64);
                audit.Property(a => a.ApprovedBy).HasMaxLength(64);
                audit.Property(a => a.Notes).HasMaxLength(256);
            });
        });
    }

    private static void ConfigureMoney<TParent>(OwnedNavigationBuilder<TParent, Money> money, string name)
        where TParent : class
    {
        money.Property(m => m.Amount).HasPrecision(18, 2).HasColumnName($"{name}Amount");
        money.Property(m => m.Currency).HasMaxLength(3).HasColumnName($"{name}Currency");
    }

    private static bool DictionaryEquals(Dictionary<string, string?>? left, Dictionary<string, string?>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null || left.Count != right.Count)
        {
            return false;
        }

        foreach (var (key, value) in left)
        {
            if (!right.TryGetValue(key, out var otherValue) || otherValue != value)
            {
                return false;
            }
        }

        return true;
    }

    private static int DictionaryHashCode(Dictionary<string, string?> value)
    {
        var hash = 0;
        foreach (var (key, val) in value)
        {
            hash = HashCode.Combine(hash, key.GetHashCode(StringComparison.Ordinal), val?.GetHashCode() ?? 0);
        }

        return hash;
    }
}
