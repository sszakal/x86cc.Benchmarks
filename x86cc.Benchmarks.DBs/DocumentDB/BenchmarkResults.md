BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 3.67GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.102
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
Job-FLJUKD : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
Job-NNAIHS : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
Job-TDMNZZ : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3
Job-GIQYWE : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3

Server=True

| Type                    | Method      | TestDataCount |                                      Mean |                          Error |      Gen0 |      Gen1 |                                        Allocated |
|-------------------------|-------------|---------------|------------------------------------------:|-------------------------------:|----------:|----------:|-------------------------------------------------:|
| CouchDbBenchmark        | Create      | 2000          |                                 4.1172 ms |                      0.0219 ms |         - |         - |                                         21.57 KB |
| MartenPostgresBenchmark | Create      | 2000          |                                 1.3273 ms |                      0.0198 ms |         - |         - |                                         19.58 KB |
| MongoDbBenchmark        | Create      | 2000          |                                 0.9731 ms |                      0.0168 ms |         - |         - |                                         36.99 KB |
| RavenDB                 | Create      | 2000          |                                  2.776 ms |                      0.0268 ms |         - |         - |                                        186.45 KB |
| CouchDbBenchmark        | Read        | 2000          |                                 1.6641 ms |                      0.0121 ms |         - |         - |                                         11.34 KB |
| MartenPostgresBenchmark | Read        | 2000          |                                 0.8992 ms |                      0.0132 ms |         - |         - |                                         10.48 KB |
| MongoDbBenchmark        | Read        | 2000          |                                 1.2046 ms |                      0.0176 ms |         - |         - |                                         44.66 KB |
| RavenDbBenchmark        | Read        | 2000          |                                  1.315 ms |                      0.0262 ms |         - |         - |                                         40.97 KB |
| CouchDbBenchmark        | Update      | 2000          |                                 4.5340 ms |                      0.0392 ms |         - |         - |                                         21.59 KB |
| MartenPostgresBenchmark | Update      | 2000          |                                 1.3701 ms |                      0.0211 ms |         - |         - |                                         19.47 KB |
| MongoDbBenchmark        | Update      | 2000          |                                 1.0898 ms |                      0.0163 ms |         - |         - |                                         41.88 KB |
| RavenDbBenchmark        | Update      | 2000          |                                  2.921 ms |                      0.0259 ms |         - |         - |                                        170.09 KB |
| CouchDbBenchmark        | Delete      | 2000          |                                 4.5075 ms |                      0.0334 ms |         - |         - |                                          4.14 KB |
| MartenPostgresBenchmark | Delete      | 2000          |                                 0.9699 ms |                      0.0191 ms |         - |         - |                                          7.71 KB |
| MongoDbBenchmark        | Delete      | 2000          |                                 0.9554 ms |                      0.0145 ms |         - |         - |                                         25.67 KB |
| RavenDbBenchmark        | Delete      | 2000          |                                  2.547 ms |                      0.0188 ms |         - |         - |                                         19.45 KB |
| CouchDbBenchmark        | Create_Bulk | 2000          |                               644.4533 ms |                     46.5715 ms |         - |         - |                                      25903.23 KB |
| MartenPostgresBenchmark | Create_Bulk | 2000          |                               393.0856 ms |                     16.5580 ms |         - |         - |                                      20273.97 KB |
| MongoDbBenchmark        | Create_Bulk | 2000          |                                79.4354 ms |                      4.3998 ms |         - |         - |                                      23402.27 KB |
| RavenDbBenchmark        | Create_Bulk | 2000          |                                535.550 ms |                     43.8781 ms | 5000.0000 | 2000.0000 |                                     350837.66 KB |
| CouchDbBenchmark        | Read_Search | 2000          |                               253.6266 ms |                      1.3610 ms |         - |         - |                                       4125.16 KB |
| MartenPostgresBenchmark | Read_Search | 2000          |                                14.1644 ms |                      0.2029 ms |   62.5000 |   15.6250 |                                       3010.48 KB |
| MongoDbBenchmark        | Read_Search | 2000          |                                15.5817 ms |                      0.0962 ms |  125.0000 |   46.8750 |                                       7380.23 KB |
| RavenDbBenchmark        | Read_Search | 2000          |                                 20.637 ms |                      0.3818 ms |  333.3333 |  166.6667 |                                      21416.97 KB |
| CouchDbBenchmark        | Update_Bulk | 2000          |                             1,108.4975 ms |                     69.6115 ms |         - |         - |                                      26250.41 KB |
| MartenPostgresBenchmark | Update_Bulk | 2000          |                               424.8535 ms |                      2.5586 ms |         - |         - |                                      20326.51 KB |
| MongoDbBenchmark        | Update_Bulk | 2000          |                               131.4418 ms |                      0.7560 ms |         - |         - |                                      33062.45 KB |
| RavenDbBenchmark        | Update_Bulk | 2000          |                                461.489 ms |                      3.6090 ms | 9000.0000 | 3000.0000 |                                     352270.29 KB |
| CouchDbBenchmark        | Delete_Bulk | 2000          |                               666.4471 ms |                     10.1985 ms |         - |         - |                                    1390943.73 KB |
| MartenPostgresBenchmark | Delete_Bulk | 2000          |                                93.8181 ms |                      1.1759 ms |         - |         - |                                       2054.27 KB |
| MongoDbBenchmark        | Delete_Bulk | 2000          |                                17.3924 ms |                      0.1737 ms |         - |         - |                                        402.54 KB |
| RavenDbBenchmark        | Delete_Bulk | 2000          |                                 61.227 ms |                      1.1147 ms |         - |         - |                                       6631.28 KB |
