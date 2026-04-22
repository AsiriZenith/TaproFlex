using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace BuildingBlocks.Extensions;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddDefaultOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment,
        string defaultServiceName)
    {
        var serviceName = configuration["OpenTelemetry:ServiceName"];
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            serviceName = defaultServiceName;
        }

        var serviceVersion = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "unknown";
        var otlpEndpoint = configuration["OpenTelemetry:Otlp:Endpoint"];
        var protocolValue = configuration["OpenTelemetry:Otlp:Protocol"];

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(serviceName: serviceName, serviceVersion: serviceVersion)
                .AddAttributes(new Dictionary<string, object>
                {
                    ["deployment.environment"] = environment.EnvironmentName
                }))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                ConfigureOtlpExporter(otlpEndpoint, protocolValue, tracing);
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation();

                ConfigureOtlpExporter(otlpEndpoint, protocolValue, metrics);
            });

        return services;
    }

    private static void ConfigureOtlpExporter(
        string? otlpEndpoint,
        string? protocolValue,
        TracerProviderBuilder tracing)
    {
        if (string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            return;
        }

        tracing.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);

            if (Enum.TryParse<OtlpExportProtocol>(protocolValue, true, out var protocol))
            {
                options.Protocol = protocol;
            }
        });
    }

    private static void ConfigureOtlpExporter(
        string? otlpEndpoint,
        string? protocolValue,
        MeterProviderBuilder metrics)
    {
        if (string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            return;
        }

        metrics.AddOtlpExporter(options =>
        {
            options.Endpoint = new Uri(otlpEndpoint);

            if (Enum.TryParse<OtlpExportProtocol>(protocolValue, true, out var protocol))
            {
                options.Protocol = protocol;
            }
        });
    }
}
