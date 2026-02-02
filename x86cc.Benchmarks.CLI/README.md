# Benchmark CLI

Run the full suite:
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release
```

Run specific benchmark categories (pass BenchmarkDotNet args after `--`):
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Caching Systems" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "AspNetCore E2E" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Document DBs" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Entity Framework DBs" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Fake Data Generators" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "IoC" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Mappers" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "MessageBrokers" --join
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Serialization" --join
```

Notes:
- Use `sudo` only if you need elevated permissions for system profiling.
