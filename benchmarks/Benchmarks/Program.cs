using BenchmarkDotNet.Running;

// https://benchmarkdotnet.org/articles/guides/how-to-run.html#benchmarkswitcher
// https://benchmarkdotnet.org/articles/guides/console-args.html
// $ dotnet run -c Release -- --list Flat
// Benchmarks.DeSerializerVsUTF8.DeSerializer
// Benchmarks.DeSerializerVsUTF8.UTF8
// $ dotnet run -c Release -- -f Benchmarks.DeSerializerVsUTF8.*
BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

// Console.WriteLine(new JsonDeSerializerVsUTF8().JsonDeSerializer());
