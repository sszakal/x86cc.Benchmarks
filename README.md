# x86cc.Benchmarks
Multi-project .NET benchmark suite covering serialization, databases, message brokers, caching, mappers, IoC containers, fake data generators, and ASP.NET Core end-to-end scenarios.

## Projects
- `x86cc.Benchmarks.CLI`: Benchmark runner that aggregates all benchmark assemblies.
- `x86cc.Benchmarks.Common`: Shared utilities and helpers.
- `x86cc.Benchmarks.DBs`: Database and document database benchmarks (Testcontainers).
- `x86cc.Benchmarks.MessageBrokers`: Message broker benchmarks (Testcontainers).
- `x86cc.Benchmarks.*`: Other benchmark suites (Serializers, Mappers, IoCs, CachingSystems, FakeDataGenerators, AspNetCore).

## Prerequisites
- .NET SDK 10 (projects target `net10.0`).
- Docker for Testcontainers-based benchmarks (DBs and MessageBrokers).

## Build and Run
Build the solution:
```sh
dotnet build x86cc.Benchmarks.slnx
```

Run all benchmarks via the CLI:
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release
```

Run a specific category via the CLI:
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Serialization" --join
```

All benchmark projects are class libraries; use the CLI to run any benchmarks.

## Benchmark Categories
- AspNetCore E2E
- Caching Systems
- Document DBs
- Entity Framework DBs
- Fake Data Generators
- IoC
- Mappers
- MessageBrokers
- Serialization

## Notes
- Benchmark results and logs are emitted under `BenchmarkDotNet.Artifacts/`.
- Custom DB images live in `x86cc.Benchmarks.DBs/docker/`.
