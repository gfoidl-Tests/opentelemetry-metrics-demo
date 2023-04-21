namespace BookStore.WebApi.Middleware;

public class SimulatedLatencyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly int _minDelayInMs;
    private readonly int _maxDelayInMs;

    public SimulatedLatencyMiddleware(
        RequestDelegate next,
        TimeSpan min,
        TimeSpan max)
    {
        _next = next;
        _minDelayInMs = (int)min.TotalMilliseconds;
        _maxDelayInMs = (int)max.TotalMilliseconds;
    }

    public async Task Invoke(HttpContext context)
    {
        int delayInMs = Random.Shared.Next(_minDelayInMs, _maxDelayInMs);
        await Task.Delay(delayInMs);

        await _next(context);
    }
}
