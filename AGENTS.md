# Repository Guidelines

## Project Structure & Module Organization
This repo is a multi-project .NET solution of benchmarks. Each benchmark area lives in its own folder (e.g., `x86cc.Benchmarks.Serializers`, `x86cc.Benchmarks.Mappers`, `x86cc.Benchmarks.DBs`). Shared utilities are in `x86cc.Benchmarks.Common`. The CLI entry point is `x86cc.Benchmarks.CLI`. The solution file is `x86cc.Benchmarks.slnx`. Testcontainers helpers live under `x86cc.Benchmarks.TestContainers`.

## Prerequisites
- .NET SDK 10 (projects target `net10.0`).
- Docker for Testcontainers-based benchmarks (DBs and MessageBrokers).

## Build, Test, and Development Commands
- Build the solution:
  ```sh
  dotnet build x86cc.Benchmarks.slnx
  ```
  Compiles all projects.
- Run the CLI:
  ```sh
  dotnet run --project x86cc.Benchmarks.CLI -c Release
  ```
  Starts the benchmark runner.
- Run a specific benchmark project:
  ```sh
  dotnet run --project x86cc.Benchmarks.DBs -c Release
  ```
  Executes that project’s benchmarks.
- Run a category via the CLI:
  ```sh
  dotnet run --project x86cc.Benchmarks.CLI -c Release -- --anyCategories "Serialization" --join
  ```

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

## Coding Style & Naming Conventions
Use standard C# conventions with 4-space indentation. Types use `PascalCase`, locals use `camelCase`. Keep benchmark classes in the `x86cc.Benchmarks.*` namespace that matches the folder. Keep filenames aligned with class names (e.g., `PostgresBenchmark.cs`).

## Testing Guidelines
Benchmarks are the primary “tests.” There are no unit test projects in this repo currently. When adding validation tests, follow `*Tests.cs` naming and place them in a dedicated test project (e.g., `x86cc.Benchmarks.Tests`). Use `dotnet test` to run tests.

## Commit & Pull Request Guidelines
No explicit commit convention is documented; use short, imperative messages (e.g., “Add Marten DB benchmark”). PRs should include:
- A brief summary of what changed and why
- Any benchmark numbers or comparison notes if performance is impacted
- Relevant setup details (e.g., Docker image tags or platform assumptions)

## Environment & Containers
Database benchmarks use Docker via Testcontainers. If you’re on Apple Silicon, ensure the expected platform or image is available. See `x86cc.Benchmarks.DBs/docker/` for custom database images.
BenchmarkDotNet emits results into `BenchmarkDotNet.Artifacts/`.
