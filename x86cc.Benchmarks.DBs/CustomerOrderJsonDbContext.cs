using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace x86cc.Benchmarks.DBs;

public sealed class CustomerOrderJsonDbContext(DbContextOptions<CustomerOrderJsonDbContext> options) : DbContext(options)
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.Never
    };

    public DbSet<CustomerOrderJsonRow> Orders => Set<CustomerOrderJsonRow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var payloadConverter = new ValueConverter<CustomerOrderRoot, string>(
            value => JsonSerializer.Serialize(value, JsonOptions),
            value => JsonSerializer.Deserialize<CustomerOrderRoot>(value, JsonOptions) ?? new CustomerOrderRoot());

        var payloadComparer = new ValueComparer<CustomerOrderRoot>(
            (left, right) => JsonSerializer.Serialize(left, JsonOptions) == JsonSerializer.Serialize(right, JsonOptions),
            value => JsonSerializer.Serialize(value, JsonOptions).GetHashCode(StringComparison.Ordinal),
            value => JsonSerializer.Deserialize<CustomerOrderRoot>(JsonSerializer.Serialize(value, JsonOptions), JsonOptions) ?? new CustomerOrderRoot());

        modelBuilder.Entity<CustomerOrderJsonRow>(order =>
        {
            order.ToTable("CustomerOrdersJson");
            order.HasKey(o => o.Id);
            order.Property(o => o.Status).HasConversion<string>().HasMaxLength(16);
            order.HasIndex(o => o.Status);
            order.HasIndex(o => o.CreatedAt);

            order.Property(o => o.Payload)
                .HasColumnType("jsonb")
                .HasConversion(payloadConverter)
                .Metadata.SetValueComparer(payloadComparer);
        });
    }
}

public sealed class CustomerOrderJsonRow
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public CustomerOrderRoot Payload { get; set; } = new();
}
