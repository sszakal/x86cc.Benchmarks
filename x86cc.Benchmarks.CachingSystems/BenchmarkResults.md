BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 3.55GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                                                         
.NET SDK 10.0.102                                                                                                                                                                                                                              
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                                                      
Job-KASRWB : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Server=True  InvocationCount=1  IterationCount=500  
UnrollFactor=1  WarmupCount=10

| Type                    | Method | Mean       | Error    | Allocated |
|------------------------ |------- |-----------:|---------:|----------:|
| DragonflyCacheBenchmark | Create |   635.3 us | 11.22 us |    9112 B |
| RedisCacheBenchmark     | Create |   610.2 us |  8.74 us |    9112 B |
| ValkeyCacheBenchmark    | Create |   599.5 us |  9.40 us |    9112 B |
| DragonflyCacheBenchmark | Read   |   522.7 us |  8.77 us |    3112 B |
| RedisCacheBenchmark     | Read   |   500.9 us |  8.24 us |    3120 B |
| ValkeyCacheBenchmark    | Read   |   497.8 us |  7.15 us |    3120 B |
| DragonflyCacheBenchmark | Update |   657.2 us | 10.81 us |    6176 B |
| RedisCacheBenchmark     | Update |   622.0 us |  9.19 us |    6208 B |
| ValkeyCacheBenchmark    | Update |   608.9 us |  9.67 us |    6176 B |
| DragonflyCacheBenchmark | Delete | 1,063.2 us | 16.99 us |     400 B |
| RedisCacheBenchmark     | Delete |   380.0 us |  4.78 us |     400 B |
| ValkeyCacheBenchmark    | Delete |   377.2 us |  4.57 us |     400 B |