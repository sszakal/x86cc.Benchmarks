using BenchmarkDotNet.Running;
using x86cc.Benchmarks.CachingSystems;
using x86cc.Benchmarks.FakeDataGenerators;
using x86cc.Benchmarks.Mappers;
using x86cc.Benchmarks.MessageBrokers;
using x86cc.Benchmarks.Serializers;
using x86cc.Benchmarks.DBs.DocumentDB;
using x86cc.Benchmarks.IoCs;

var summary = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly)
     .With(typeof(MessageBrokerBenchmark).Assembly)
     .With(typeof(FakeDataGeneratorBenchmark).Assembly)
     .With(typeof(SerializationBenchmark).Assembly)
     .With(typeof(MapperBenchmark).Assembly)
     .With(typeof(DocumentDbBenchmark).Assembly)
     .With(typeof(CacheBenchmark).Assembly)
     .With(typeof(IoCBenchmark).Assembly)
     .Run(args);