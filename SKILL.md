# x86cc.Benchmarks Skill Guide
Practical notes for working on benchmarks in this repo.

## Quick Commands
Build everything:
```sh
dotnet build x86cc.Benchmarks.slnx
```

Run the full CLI suite:
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release
```

Run a category via the CLI:
```sh
dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Serialization" --join
```

All benchmark projects are class libraries; use the CLI to run benchmarks.

## Adding or Updating Benchmarks
- Place new benchmarks in the matching project folder and namespace (`x86cc.Benchmarks.*`).
- Keep filenames aligned with class names (e.g., `PostgresBenchmark.cs`).
- Use BenchmarkDotNet attributes like `[Benchmark]`, `[GlobalSetup]`, and `[BenchmarkCategory("...")]`.
- For DBs and MessageBrokers, use Testcontainers and shared helpers in `x86cc.Benchmarks.TestContainers` where possible.

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

## Outputs and Artifacts
- BenchmarkDotNet outputs results to `BenchmarkDotNet.Artifacts/`.
- Build outputs are under `bin/` and `obj/` for each project.
