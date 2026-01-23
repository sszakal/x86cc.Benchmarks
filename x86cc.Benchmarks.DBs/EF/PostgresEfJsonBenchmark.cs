using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Testcontainers.PostgreSql;
using x86cc.Benchmarks.DBs;

namespace x86cc.Benchmarks.DBs.EF;

public class PostgresEfJsonBenchmark : EFBenchmark
{
    private const string DatabaseName = "benchmarks";
    private const string Username = "bench";
    private const string Password = "bench_pw";

    private DbContextOptions<CustomerOrderJsonDbContext>? _options;

    protected override IContainer BuildContainer()
    {
        var builder = new PostgreSqlBuilder("postgres:18")
            .WithDatabase(DatabaseName)
            .WithUsername(Username)
            .WithPassword(Password);

        var platform = ResolveDockerPlatform();
        if (!string.IsNullOrWhiteSpace(platform))
        {
            builder = builder.WithCreateParameterModifier(p => p.Platform = platform);
        }

        return builder.Build();
    }

    protected override string GetConnectionString(IContainer container)
    {
        return ((PostgreSqlContainer)container).GetConnectionString();
    }

    protected override Task InitializeAsync(IContainer container)
    {
        _options = new DbContextOptionsBuilder<CustomerOrderJsonDbContext>()
            .UseNpgsql(GetConnectionString(container))
            .EnableDetailedErrors(false)
            .Options;
        return Task.CompletedTask;
    }

    protected override async Task ResetDatabaseAsync()
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteCreateAsync(CustomerOrderRoot item)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        context.Orders.Add(CreateRow(item));
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task CreateBulkAsync(CustomerOrderRoot[] items)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var rows = items.Select(CreateRow).ToArray();
        context.Orders.AddRange(rows);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var row = CreateRow(order);
        context.Orders.Update(row);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var rows = orders.Select(CreateRow).ToArray();
        context.Orders.UpdateRange(rows);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task SearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        _ = await context.Orders.AsNoTracking()
            .Where(o => o.Status == OrderStatus.Submitted)
            .OrderBy(o => o.CreatedAt)
            .Take(take)
            .ToArrayAsync()
            .ConfigureAwait(false);
    }

    protected override async Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var row = await context.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
        return row?.Payload;
    }

    protected override async Task DeleteOneAsync(CustomerOrderRoot order)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var row = new CustomerOrderJsonRow { Id = order.Id };
        context.Orders.Attach(row);
        context.Orders.Remove(row);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task DeleteBulkAsync(CustomerOrderRoot[] orders)
    {
        await using var context = new CustomerOrderJsonDbContext(_options!);
        var rows = orders.Select(o => new CustomerOrderJsonRow { Id = o.Id }).ToArray();
        context.Orders.AttachRange(rows);
        context.Orders.RemoveRange(rows);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    private static CustomerOrderJsonRow CreateRow(CustomerOrderRoot order)
    {
        return new CustomerOrderJsonRow
        {
            Id = order.Id,
            Status = order.Status,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Payload = order
        };
    }
}
