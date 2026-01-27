BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 3.63GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                                                                     
.NET SDK 10.0.102                                                                                                                                                                                                                                          
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                                                                  
Job-PMNSIJ : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Runtime=.NET 10.0  IterationCount=40  WarmupCount=5

| Type                         | Method      | ObjectCount | Mean        | Error     | StdDev     | Median      | Gen0        | Gen1       | Gen2     | Allocated  |
|----------------------------- |------------ |------------ |------------:|----------:|-----------:|------------:|------------:|-----------:|---------:|-----------:|
| MessagePackBenchmarks        | Serialize   | 10000       |    56.63 ms |  0.241 ms |   0.416 ms |    56.63 ms |   1777.7778 |  1666.6667 | 111.1111 |   13.37 MB |
| NewtonSerializationBenchmark | Serialize   | 10000       |   198.20 ms |  2.601 ms |   4.274 ms |   197.31 ms |  29333.3333 |  7000.0000 | 333.3333 |  232.35 MB |
| ProtoBufBenchmarks           | Serialize   | 10000       |    77.50 ms |  0.307 ms |   0.512 ms |    77.47 ms |   5142.8571 |  2142.8571 | 142.8571 |   40.24 MB |
| SystemSerializationBenchmark | Serialize   | 10000       |   130.81 ms |  0.698 ms |   1.222 ms |   130.75 ms |  12666.6667 |  4333.3333 | 333.3333 |   98.62 MB |
| YamlDotNetBenchmark          | Serialize   | 10000       | 3,545.20 ms | 15.717 ms |  27.112 ms | 3,540.04 ms | 224000.0000 | 19000.0000 |        - |  1786.8 MB |
| MessagePackBenchmarks        | Deserialize | 10000       |   190.16 ms |  1.529 ms |   2.638 ms |   190.08 ms |  10000.0000 |  5333.3333 | 666.6667 |   75.48 MB |
| NewtonSerializationBenchmark | Deserialize | 10000       |   340.30 ms |  6.389 ms |  11.190 ms |   335.23 ms |  20000.0000 |  6000.0000 |        - |  162.05 MB |
| ProtoBufBenchmarks           | Deserialize | 10000       |   161.98 ms |  1.548 ms |   2.712 ms |   161.31 ms |   7500.0000 |  7000.0000 | 500.0000 |   58.35 MB |
| SystemSerializationBenchmark | Deserialize | 10000       |   273.73 ms |  3.855 ms |   6.546 ms |   271.73 ms |  13000.0000 |  6000.0000 |        - |  106.12 MB |
| YamlDotNetBenchmark          | Deserialize | 10000       | 3,299.75 ms | 58.046 ms | 103.176 ms | 3,231.95 ms | 276000.0000 | 21000.0000 |        - | 2206.23 MB |