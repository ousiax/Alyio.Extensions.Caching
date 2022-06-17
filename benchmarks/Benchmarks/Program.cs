using BenchmarkDotNet.Running;
using Benchmarks;

BenchmarkRunner.Run<DeSerializerVsUTF8>();
// Console.WriteLine(new JsonDeSerializerVsUTF8().JsonDeSerializer());
