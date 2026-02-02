using JasperFx;
using Marten;
using Microsoft.Extensions.Hosting;

namespace x86cc.Benchmarks.AspNetCore.Repositories;

public sealed class MartenSchemaInitializer(IDocumentStore store) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await store.Storage.ApplyAllConfiguredChangesToDatabaseAsync(AutoCreate.All).ConfigureAwait(false);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
