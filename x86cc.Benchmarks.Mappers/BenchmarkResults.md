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


BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 9 9950X 0.62GHz, 1 CPU, 32 logical and 16 physical cores                                                                                                                                                                                                                                                                                                                                                                
.NET SDK 10.0.102                                                                                                                                                                                                                                                                                                                                                                                                                 
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4                                                                                                                                                                                                                                                                                                                                                         
Job-BPFIQH : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4

Runtime=.NET 10.0  IterationCount=20  WarmupCount=2

| Type                | Method      | ObjectCount | Mean     | Error    | StdDev   | Gen0      | Gen1      | Gen2     | Allocated |
|-------------------- |------------ |------------ |---------:|---------:|---------:|----------:|----------:|---------:|----------:|
| AutoMapperBenchmark | MapOneByOne | 10000       | 67.94 ms | 0.454 ms | 0.505 ms | 3625.0000 | 3500.0000 | 500.0000 |  51.43 MB |                                                                                                                                                                                                                                                                                               
| MapperlyBenchmark   | MapOneByOne | 10000       | 62.62 ms | 3.183 ms | 3.406 ms | 3555.5556 | 3444.4444 | 444.4444 |  49.71 MB |
| MapsterBenchmark    | MapOneByOne | 10000       | 59.61 ms | 1.217 ms | 1.302 ms | 3500.0000 | 3375.0000 | 375.0000 |  50.98 MB |
| NoMapperBenchmarks  | MapOneByOne | 10000       | 71.79 ms | 8.371 ms | 9.640 ms | 3375.0000 | 3250.0000 | 375.0000 |  48.38 MB |
| AutoMapperBenchmark | MapMany     | 10000       | 70.37 ms | 1.838 ms | 2.043 ms | 3625.0000 | 3500.0000 | 500.0000 |  51.18 MB |
| MapperlyBenchmark   | MapMany     | 10000       | 60.68 ms | 1.035 ms | 1.108 ms | 3444.4444 | 3333.3333 | 444.4444 |  49.46 MB |
| MapsterBenchmark    | MapMany     | 10000       | 62.22 ms | 1.032 ms | 1.059 ms | 3555.5556 | 3444.4444 | 444.4444 |  50.72 MB |
| NoMapperBenchmarks  | MapMany     | 10000       | 67.31 ms | 1.729 ms | 1.922 ms | 3428.5714 | 3285.7143 | 428.5714 |  48.13 MB |