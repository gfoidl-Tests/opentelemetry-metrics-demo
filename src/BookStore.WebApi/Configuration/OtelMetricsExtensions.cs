using OpenTelemetry.Metrics;

namespace BookStore.Infrastructure.Metrics;

internal static class OtelMetricsExtensions
{
    public static MeterProviderBuilder AddOtelMetrics(this MeterProviderBuilder builder, OtelMetrics meters)
    {
        builder
            .AddMeter(meters.MetricName)
            .AddView(
                instrumentName: "orders-price",
                new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 15, 30, 45, 60, 75 } })
            .AddView(
                instrumentName: "orders-number-of-books",
                new ExplicitBucketHistogramConfiguration { Boundaries = new double[] { 1, 2, 5 } });

        return builder;
    }
}
