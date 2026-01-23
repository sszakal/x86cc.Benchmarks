using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;

namespace x86cc.Benchmarks.Common;

public class DefaultBenchmarkConfig : ManualConfig
{
    public DefaultBenchmarkConfig()
    {
        HideColumns(StatisticColumn.AllStatistics.Except([StatisticColumn.Min, StatisticColumn.Max, StatisticColumn.Mean]).ToArray());
        AddLogicalGroupRules(BenchmarkLogicalGroupRule.ByMethod);
        WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
    }
}