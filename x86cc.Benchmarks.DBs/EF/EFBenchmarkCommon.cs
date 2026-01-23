using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace x86cc.Benchmarks.DBs.EF;

public abstract class EFBenchmarkCommon : EFBenchmark
{
    private DbContextOptions<CustomerOrderDbContext>? _options;

    protected override Task InitializeAsync(IContainer container)
    {
        _options = CreateOptions(GetConnectionString(container));
        return Task.CompletedTask;
    }

    protected abstract DbContextOptions<CustomerOrderDbContext> CreateOptions(string connectionString);

    protected override async Task ResetDatabaseAsync()
    {
        await using var context = new CustomerOrderDbContext(_options!);
        await context.Database.EnsureDeletedAsync().ConfigureAwait(false);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteCreateAsync(CustomerOrderRoot item)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.Add(item);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task CreateBulkAsync(CustomerOrderRoot[] items)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.AddRange(items);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateAsync(CustomerOrderRoot order)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.Update(order);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task ExecuteUpdateBulkAsync(CustomerOrderRoot[] orders)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.UpdateRange(orders);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task SearchAsync(Expression<Func<CustomerOrderRoot, bool>> filter, int take)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        var results = await context.Orders.AsNoTracking()
            .Where(filter)
            .OrderBy(o => o.CreatedAt)
            .Take(take)
            .ToArrayAsync()
            .ConfigureAwait(false);
        
        if (results.Length != 0) return;
        throw new Exception("No results found");
    }

    protected override async Task<CustomerOrderRoot?> ExecuteLoadByIdAsync(Guid id)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        return await context.Orders.AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == id)
            .ConfigureAwait(false);
    }

    protected override async Task DeleteOneAsync(CustomerOrderRoot order)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.Remove(order);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    protected override async Task DeleteBulkAsync(CustomerOrderRoot[] orders)
    {
        await using var context = new CustomerOrderDbContext(_options!);
        context.Orders.RemoveRange(orders);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }
}
