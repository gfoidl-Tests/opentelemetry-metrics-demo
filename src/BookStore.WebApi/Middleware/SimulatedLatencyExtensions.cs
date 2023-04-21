namespace BookStore.WebApi.Middleware;

public static class SimulatedLatencyExtensions
{
    public static IApplicationBuilder UseSimulatedLatency(
        this IApplicationBuilder app,
        TimeSpan min,
        TimeSpan max)
        => app.UseMiddleware<SimulatedLatencyMiddleware>(min, max);
}
