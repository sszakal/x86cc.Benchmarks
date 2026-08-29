BenchmarkDotNet v0.15.8, Linux Arch Linux
AMD Ryzen 9 9950X 5.30GHz, 1 CPU, 32 logical and 16 physical cores                                                                                                                                       
.NET SDK 10.0.102                                                                                                                                                                                        
[Host]     : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4                                                                                                                                
Job-UFXZUX : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4                                                                                                                                
Job-WFXELE : .NET 10.0.2 (10.0.2, 10.0.225.61305), X64 RyuJIT x86-64-v4

Server=True

| Method | Job        | InvocationCount | UnrollFactor | Scenario      | Mean        | Error      | Gen0   | Allocated |
|------- |----------- |---------------- |------------- |-------------- |------------:|-----------:|-------:|----------:|                                                                              
| Create | Job-UFXZUX | Default         | 16           | F/W/L/My/NC/P |    99.89 us |   2.446 us |      - |   30.9 KB |
| Create | Job-UFXZUX | Default         | 16           | F/W/MS/My/C/P |   153.78 us |   3.044 us |      - |  31.84 KB |
| Create | Job-UFXZUX | Default         | 16           | F/W/Dr/My/C/P |   155.51 us |   3.077 us |      - |  31.94 KB |
| Create | Job-UFXZUX | Default         | 16           | F/W/L/Mp/C/P  |   156.33 us |   3.080 us |      - |  32.11 KB |
| Create | Job-UFXZUX | Default         | 16           | F/M/L/My/C/P  |   156.34 us |   3.123 us |      - |  30.96 KB |
| Create | Job-UFXZUX | Default         | 16           | F/W/L/My/C/P  |   159.33 us |   3.151 us |      - |  32.08 KB |
| Create | Job-UFXZUX | Default         | 16           | C/W/L/My/C/P  |   177.56 us |   6.172 us | 0.4883 |  37.23 KB |
|        |            |                 |              |               |             |            |        |           |
| Get    | Job-UFXZUX | Default         | 16           | F/W/Dr/My/C/P |    38.91 us |   1.836 us | 0.2441 |  18.24 KB |
| Get    | Job-UFXZUX | Default         | 16           | F/M/L/My/C/P  |    39.21 us |   1.910 us | 0.1221 |  17.24 KB |
| Get    | Job-UFXZUX | Default         | 16           | F/W/MS/My/C/P |    39.88 us |   1.785 us | 0.2441 |  18.16 KB |
| Get    | Job-UFXZUX | Default         | 16           | F/W/L/My/C/P  |    40.80 us |   1.827 us | 0.2441 |  18.38 KB |
| Get    | Job-UFXZUX | Default         | 16           | F/W/L/Mp/C/P  |    40.84 us |   1.934 us | 0.2441 |  18.42 KB |
| Get    | Job-UFXZUX | Default         | 16           | C/W/L/My/C/P  |    42.72 us |   1.858 us | 0.2441 |  21.35 KB |
| Get    | Job-UFXZUX | Default         | 16           | F/W/L/My/NC/P |    44.54 us |   1.142 us | 0.2441 |  21.48 KB |
|        |            |                 |              |               |             |            |        |           |
| Search | Job-UFXZUX | Default         | 16           | F/M/L/My/C/P  |   201.64 us |   3.995 us | 0.9766 |  74.94 KB |
| Search | Job-UFXZUX | Default         | 16           | F/W/Dr/My/C/P |   206.62 us |   4.124 us | 0.9766 |  75.93 KB |
| Search | Job-UFXZUX | Default         | 16           | F/W/L/My/NC/P |   210.02 us |   4.185 us | 0.9766 |  76.08 KB |
| Search | Job-UFXZUX | Default         | 16           | F/W/MS/My/C/P |   210.05 us |   4.193 us | 0.9766 |  75.72 KB |
| Search | Job-UFXZUX | Default         | 16           | F/W/L/Mp/C/P  |   214.04 us |   4.231 us | 0.9766 |  76.63 KB |
| Search | Job-UFXZUX | Default         | 16           | F/W/L/My/C/P  |   214.31 us |   4.256 us | 0.9766 |  76.07 KB |
| Search | Job-UFXZUX | Default         | 16           | C/W/L/My/C/P  |   221.13 us |   4.183 us | 0.9766 |  79.95 KB |
|        |            |                 |              |               |             |            |        |           |
| Edit   | Job-UFXZUX | Default         | 16           | F/W/L/My/NC/P |   136.06 us |   2.710 us | 0.4883 |  37.02 KB |
| Edit   | Job-UFXZUX | Default         | 16           | F/W/Dr/My/C/P |   182.75 us |   3.943 us |      - |  35.07 KB |
| Edit   | Job-UFXZUX | Default         | 16           | F/W/L/Mp/C/P  |   187.38 us |   3.860 us |      - |  35.26 KB |
| Edit   | Job-UFXZUX | Default         | 16           | F/W/MS/My/C/P |   189.21 us |   7.385 us |      - |  35.01 KB |
| Edit   | Job-UFXZUX | Default         | 16           | F/M/L/My/C/P  |   189.81 us |   4.138 us |      - |  34.08 KB |
| Edit   | Job-UFXZUX | Default         | 16           | F/W/L/My/C/P  |   194.66 us |   3.872 us |      - |  35.22 KB |
| Edit   | Job-UFXZUX | Default         | 16           | C/W/L/My/C/P  |   206.51 us |   4.113 us | 0.4883 |  41.12 KB |
|        |            |                 |              |               |             |            |        |           |
| Delete | Job-WFXELE | 1               | 1            | F/W/L/My/NC/M |          NA |         NA |     NA |        NA |
| Delete | Job-WFXELE | 1               | 1            | F/W/L/My/NC/P |   509.51 us |  43.892 us |      - |  22.94 KB |
| Delete | Job-WFXELE | 1               | 1            | F/W/MS/My/C/P |   546.07 us |  31.489 us |      - |   23.2 KB |
| Delete | Job-WFXELE | 1               | 1            | F/W/L/My/C/P  |   622.19 us |  48.757 us |      - |   23.5 KB |
| Delete | Job-WFXELE | 1               | 1            | F/W/L/Mp/C/P  |   664.42 us |  64.231 us |      - |   23.5 KB |
| Delete | Job-WFXELE | 1               | 1            | F/W/Dr/My/C/P |   836.06 us |  88.358 us |      - |  23.28 KB |
| Delete | Job-WFXELE | 1               | 1            | F/M/L/My/C/P  |   966.86 us |  95.687 us |      - |  22.53 KB |
| Delete | Job-WFXELE | 1               | 1            | C/W/L/My/C/P  | 1,068.22 us | 138.735 us |      - |  26.52 KB |