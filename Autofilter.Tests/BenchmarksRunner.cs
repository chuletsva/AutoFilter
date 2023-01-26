using Autofilter.Tests.Benchmarks;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit.Abstractions;

namespace Autofilter.Tests;

public class BenchmarksRunner
{
    private readonly ITestOutputHelper mLogger;

    public BenchmarksRunner(ITestOutputHelper logger)
    {
        mLogger = logger;
    }

    [Fact]
    public void RunAutoFilterBenchmarks()
    {
        RunBenchmark<AutoFilterBenchmarks>();
    }

    private void RunBenchmark<TBenchmark>()
    {
        var summary = BenchmarkRunner.Run<TBenchmark>();

        var logger = new AccumulationLogger();

        MarkdownExporter.Console.ExportToLog(summary, logger);

        mLogger.WriteLine(logger.GetLog());
    }
}