BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 7 5700G with Radeon Graphics 3.67GHz, 1 CPU, 16 logical and 8 physical cores                                                                                                                   
.NET SDK 10.0.102                                                                                                                                                                                        
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v3                                                                                                                                

Server=True  InvocationCount=1  UnrollFactor=1

| Type                    | Method      |         Mean |       Error |  Allocated |
|-------------------------|-------------|-------------:|------------:|-----------:|
| MySqlEfBenchmark        | Create      |     6.245 ms |   0.0700 ms | 1188.24 KB |
| OracleEfBenchmark       | Create      |    10.100 ms |   0.3355 ms |  991.48 KB |
| PostgresEfBenchmark     | Create      |     3.610 ms |   0.0776 ms |  823.09 KB |
| PostgresEfJsonBenchmark | Create      |     1.808 ms |   0.0448 ms |  127.34 KB |
| SqlServerEfBenchmark    | Create      |     7.003 ms |   0.4009 ms |  870.73 KB |
| MySqlEfBenchmark        | Read        |     3.214 ms |   0.0671 ms | 3046.59 KB |
| OracleEfBenchmark       | Read        |     2.191 ms |   0.0688 ms |  232.98 KB |
| PostgresEfBenchmark     | Read        |     2.271 ms |   0.0318 ms |  174.39 KB |
| PostgresEfJsonBenchmark | Read        |     1.451 ms |   0.0256 ms |   69.88 KB |
| SqlServerEfBenchmark    | Read        |     2.017 ms |   0.0386 ms |  478.38 KB |
| MySqlEfBenchmark        | Update      |     5.566 ms |   0.1182 ms | 1167.61 KB |
| OracleEfBenchmark       | Update      |    15.373 ms |   0.5514 ms | 1012.05 KB |
| PostgresEfBenchmark     | Update      |     3.356 ms |   0.0701 ms |   717.1 KB |
| PostgresEfJsonBenchmark | Update      |     1.903 ms |   0.0435 ms |  127.67 KB |
| SqlServerEfBenchmark    | Update      |     9.831 ms |   1.0200 ms |  791.34 KB |
| MySqlEfBenchmark        | Delete      |     7.232 ms |   0.1349 ms |  835.66 KB |
| OracleEfBenchmark       | Delete      |    11.447 ms |   0.4337 ms |  748.63 KB |
| PostgresEfBenchmark     | Delete      |     3.330 ms |   0.0662 ms |  639.66 KB |
| PostgresEfJsonBenchmark | Delete      |     1.368 ms |   0.0266 ms |   56.57 KB |
| SqlServerEfBenchmark    | Delete      |    12.532 ms |   0.7409 ms |  728.25 KB |
| MySqlEfBenchmark        | Create_Bulk | 3,254.847 ms | 347.1722 ms |  628.06 MB |
| OracleEfBenchmark       | Create_Bulk | 4,852.591 ms | 645.1032 ms |  568.85 MB |
| PostgresEfBenchmark     | Create_Bulk | 1,571.014 ms |  11.8598 ms |  518.81 MB |
| PostgresEfJsonBenchmark | Create_Bulk |   256.157 ms |   1.7306 ms |   64.85 MB |
| SqlServerEfBenchmark    | Create_Bulk | 2,430.250 ms |  14.1817 ms |  492.88 MB |
| MySqlEfBenchmark        | Read_Search |    324.94 ms |    8.243 ms |  261.48 MB |  
| OracleEfBenchmark       | Read_Search |   133.854 ms |   0.4608 ms |   14.64 MB |
| PostgresEfBenchmark     | Read_Search |    36.241 ms |   0.3634 ms |    9.47 MB |
| PostgresEfJsonBenchmark | Read_Search |     6.347 ms |   0.1738 ms |    3.27 MB |
| SqlServerEfBenchmark    | Read_Search |   129.011 ms |   1.4164 ms |   23.05 MB |
| MySqlEfBenchmark        | Update_Bulk |  3,715.30 ms |    6.797 ms |  705.76 MB |
| OracleEfBenchmark       | Update_Bulk |  6,118.97 ms |   45.074 ms |  592.51 MB |
| PostgresEfBenchmark     | Update_Bulk |  1,624.61 ms |    7.817 ms |  464.63 MB |
| PostgresEfJsonBenchmark | Update_Bulk |    277.07 ms |    1.371 ms |   65.86 MB |
| SqlServerEfBenchmark    | Update_Bulk |  2,424.48 ms |   17.650 ms |  463.53 MB |
| MySqlEfBenchmark        | Delete_Bulk |  3,138.18 ms |   18.570 ms |   779.5 MB |
| OracleEfBenchmark       | Delete_Bulk |  6,201.74 ms |   34.645 ms |  733.75 MB |
| PostgresEfBenchmark     | Delete_Bulk |  1,588.35 ms |    9.727 ms |  640.62 MB |
| PostgresEfJsonBenchmark | Delete_Bulk |     57.24 ms |    0.475 ms |      14 MB |
| SqlServerEfBenchmark    | Delete_Bulk |  3,301.95 ms |  111.396 ms |  652.24 MB |

