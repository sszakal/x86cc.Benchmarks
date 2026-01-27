BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 2.39GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                                                                     
.NET SDK 10.0.102                                                                                                                                                                                                                                          
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                                                                  
Job-JVQKKY : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Runtime=.NET 10.0  IterationCount=10  WarmupCount=2

| Type                  | Method            | ObjectCount | Mean        | Error     | Gen0        | Gen1       | Gen2      | Allocated  |
|---------------------- |------------------ |------------ |------------:|----------:|------------:|-----------:|----------:|-----------:|
| AutoFixture           | 'Generate Single' | 10000       | 1,927.64 ms | 25.220 ms | 192000.0000 |  5000.0000 | 1000.0000 | 1530.96 MB |   
| AutoFixtureUsingBogus | 'Generate Single' | 10000       |    66.10 ms |  0.919 ms |   5333.3333 |  2000.0000 |  333.3333 |   42.56 MB |
| Bogus                 | 'Generate Single' | 10000       |    65.01 ms |  2.106 ms |   5750.0000 |  2250.0000 |  500.0000 |   42.02 MB |
| AutoFixture           | 'Generate Many'   | 10000       | 1,889.93 ms | 27.156 ms | 195000.0000 | 19000.0000 | 3000.0000 | 1538.54 MB |
| AutoFixtureUsingBogus | 'Generate Many'   | 10000       |    62.86 ms |  0.780 ms |   5500.0000 |  1625.0000 |  250.0000 |   42.44 MB |
| Bogus                 | 'Generate Many'   | 10000       |    59.51 ms |  0.508 ms |   5555.5556 |  2000.0000 |  333.3333 |   41.78 MB |