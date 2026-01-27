BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 2.39GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                                                                     
.NET SDK 10.0.102                                                                                                                                                                                                                                          
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                                                                  
Job-FQZTRT : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Runtime=.NET 10.0  Server=True  IterationCount=5 WarmupCount=1

| Type              | Method            | MessageCount | MessageSize | DurableQueue | PublisherConfirmationsEnabled | PublisherConfirmationTrackingEnabled | Mean     | Error    | StdDev   | Exceptions |
|------------------ |------------------ |------------- |------------ |------------- |------------------------------ |------------------------------------- |---------:|---------:|---------:|-----------:|
| LavinMQBenchmark  | OneMessageAtATime | 100000       | 8192        | True         | True                          | True                                 | 11.584 s | 0.4138 s | 0.1075 s |          - |
| RabbitMQBenchmark | OneMessageAtATime | 100000       | 8192        | True         | True                          | True                                 | 22.659 s | 0.0978 s | 0.0151 s |          - |
| LavinMQBenchmark  | BulkPublish       | 100000       | 8192        | True         | True                          | True                                 |  3.816 s | 0.0630 s | 0.0164 s |          - |
| RabbitMQBenchmark | BulkPublish       | 100000       | 8192        | True         | True                          | True                                 |  2.842 s | 0.2518 s | 0.0654 s |          - |
| LavinMQBenchmark  | MessageLatency    | 100000       | 8192        | True         | True                          | True                                 | 14.811 s | 0.3074 s | 0.0798 s |          - |
| RabbitMQBenchmark | MessageLatency    | 100000       | 8192        | True         | True                          | True                                 | 22.280 s | 0.6760 s | 0.1755 s |          - |