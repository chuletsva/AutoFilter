using Autofilter.Rules;
using Autofilter.Tests.EF.Common;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Loggers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Autofilter.Tests.EF;

public class BenchmarkRunner
{
    private readonly ITestOutputHelper mLogger;

    public BenchmarkRunner(ITestOutputHelper logger)
    {
        mLogger = logger;
    }

    [Fact]
    public void RunFilterProcessorBenchmarks()
    {
        RunBenchmark<FilterProcessorBenchmarks>();
    }

    private void RunBenchmark<TBenchmark>()
    {
        var summary = BenchmarkDotNet.Running.BenchmarkRunner.Run<TBenchmark>();

        var logger = new AccumulationLogger();

        MarkdownExporter.Console.ExportToLog(summary, logger);

        mLogger.WriteLine(logger.GetLog());
    }
}

[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[MinColumn]
[MaxColumn]
public class FilterProcessorBenchmarks
{
    private AutoFilter? _filter;
    private AutoFilter? _filter_with_select;
    private DatabaseFixture _db;

    [GlobalSetup]
    public async Task Setup()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        _filter = new AutoFilter
        (
            Filter: new FilterRule
            (
                new[]
                {
                    new Condition
                    (
                        Name: nameof(Product.Name),
                        SearchOperator: SearchOperator.StartsWith,
                        Value: "Snickers"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.Or,
                        Name: nameof(Product.Name),
                        SearchOperator: SearchOperator.Contains,
                        Value: "Mars"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.And,
                        Name: nameof(Product.ExpireDate),
                        SearchOperator: SearchOperator.GreaterOrEqual,
                        Value: DateTime.UtcNow.ToString("s")
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.And,
                        Name: nameof(Product.IsForSale),
                        SearchOperator: SearchOperator.Equals,
                        Value: "true"
                    ),
                    new Condition
                    (
                        LogicOperator: LogicOperator.Or,
                        Name: nameof(Product.IsInStock),
                        SearchOperator: SearchOperator.Equals,
                        Value: "true"
                    ),
                },

                new[]
                {
                    new Group
                    (
                        Start: 1,
                        End: 2,
                        Level: 1
                    ),
                    new Group
                    (
                        Start: 1,
                        End: 3,
                        Level: 2
                    ),
                    new Group
                    (
                        Start: 4,
                        End: 5,
                        Level: 2
                    )
                }
            )
        );

        _filter_with_select = _filter with
        {
            Select = new[]
            {
                nameof(Product.Id),
                nameof(Product.Name)
            }
        };

        _db = new DatabaseFixture();

        await _db.InitializeAsync();

        _db.DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
    }

    [GlobalCleanup]
    public async Task Cleanup()
    {
        await _db.DisposeAsync();
    }

    [Benchmark(Baseline = true)]
    public Task Plain()
    {
        return _db.DbContext.Products.AsNoTracking().Where(x =>
            ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow) &&
            (x.IsForSale || x.IsInStock)).ToArrayAsync();
    }

    [Benchmark]
    public Task AutoFilter()
    {
        return _db.DbContext.Products.AsNoTracking().ApplyFilter(_filter).ToArrayAsync();
    }

    [Benchmark(Baseline = true)]
    public Task Plain_Select()
    {
        return _db.DbContext.Products.AsNoTracking().Where(x =>
            ((x.Name.StartsWith("Snickers") || x.Name.Contains("Mars")) && x.ExpireDate >= DateTime.UtcNow) &&
            (x.IsForSale || x.IsInStock)).Select(x => new
        {
            Id = x.Id,
            Name = x.Name,
        }).ToArrayAsync();
    }

    [Benchmark]
    public Task AutoFilter_Select()
    {
        return _db.DbContext.Products.AsNoTracking().ApplyFilterDynamic(_filter_with_select).OfType<object>().ToArrayAsync();
    }
}