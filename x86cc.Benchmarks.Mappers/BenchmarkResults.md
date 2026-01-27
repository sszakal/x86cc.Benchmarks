BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 3.62GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                                                                     
.NET SDK 10.0.102                                                                                                                                                                                                                                          
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                                                                  
Job-BPFIQH : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Runtime=.NET 10.0  IterationCount=20  WarmupCount=2

| Type                | Method      | ObjectCount | Mean      | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|-------------------- |------------ |------------ |----------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoMapperBenchmark | MapOneByOne | 10000       | 102.85 ms | 0.886 ms | 0.985 ms | 6800.0000 | 3600.0000 | 400.0000 |  51.44 MB |
| MapperlyBenchmark   | MapOneByOne | 10000       |  84.88 ms | 0.685 ms | 0.762 ms | 6500.0000 | 3333.3333 | 333.3333 |  49.71 MB |
| MapsterBenchmark    | MapOneByOne | 10000       |  97.80 ms | 7.764 ms | 8.629 ms | 6600.0000 | 3400.0000 | 400.0000 |  50.98 MB |
| NoMapperBenchmarks  | MapOneByOne | 10000       |  91.93 ms | 1.407 ms | 1.564 ms | 6333.3333 | 3333.3333 | 333.3333 |  48.38 MB |
| AutoMapperBenchmark | MapMany     | 10000       | 101.61 ms | 0.478 ms | 0.531 ms | 6800.0000 | 3600.0000 | 400.0000 |  51.19 MB |
| MapperlyBenchmark   | MapMany     | 10000       |  84.66 ms | 0.943 ms | 1.009 ms | 6500.0000 | 3333.3333 | 333.3333 |  49.46 MB |
| MapsterBenchmark    | MapMany     | 10000       |  93.05 ms | 4.618 ms | 4.941 ms | 6600.0000 | 3400.0000 | 400.0000 |  50.73 MB |
| NoMapperBenchmarks  | MapMany     | 10000       |  94.03 ms | 2.902 ms | 3.226 ms | 6333.3333 | 3333.3333 | 333.3333 |  48.13 MB |