using BookStore.Infrastructure.Metrics;
using BookStore.WebApi.Middleware;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "BookStore API",
        Version = "v1"
    });
});

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.RegisterInfrastureDependencies(builder.Configuration);

OtelMetrics meters = new();
builder.Services.AddSingleton(meters);

builder.Services
    .AddOpenTelemetry()
    .WithMetrics(meterProviderBuilder => meterProviderBuilder
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("BookStore.WebApi"))
        .AddOtelMetrics(meters)     // Custom metrics
        .AddAspNetCoreInstrumentation()
        .AddProcessInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"] ?? throw new InvalidOperationException());
        })
    );

WebApplication app = builder.Build();

// Add simulated latency to improve http requests avg. time dashboard
app.UseSimulatedLatency(
    min: TimeSpan.FromMilliseconds(500),
    max: TimeSpan.FromMilliseconds(1000)
);
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
