using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TelemetryInstaller
{
    public static class TelemetryInstaller
    {
        private static OpenTelemetryBuilder AddOpenTelemetry(IServiceCollection provider, IHostEnvironment env, ResourceBuilder resourceBuilder, string endpoint)
        {
            return provider.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                            options.EnrichWithException = (activity, exp) =>
                            {
                                activity.SetTag("exception.message", exp.Message);
                                activity.SetTag("exception.stacktrace", exp.StackTrace);
                                activity.SetCustomProperty("exception.value", exp);
                            };
                            options.EnrichWithHttpRequest = (activity, request) =>
                            {
                                activity.SetTag("http.user_agent", request.Headers.UserAgent.ToString());
                            };
                        })
                        .AddEntityFrameworkCoreInstrumentation()
                        .AddHttpClientInstrumentation();
                    if (env.IsDevelopment())
                    {
                        tracing.AddConsoleExporter();
                    }

                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        opt.Endpoint = new Uri(endpoint);
                    });
                });
        }

        public static IServiceCollection AddTelemetry(this IServiceCollection provider, IHostEnvironment env)
        {
            string? endpoint = Environment.GetEnvironmentVariable("TELEMETRY_OLTP_ADDRESS");
            if (endpoint == null)
            {
                return provider;
            }

            Console.WriteLine($"Added {endpoint} telemetry");
            ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault()
                    .AddService(serviceName: env.ApplicationName)
                    .AddAttributes(new Dictionary<string, object>
                    {
                        ["environment"] = env.EnvironmentName,
                        ["version"] = "1.0.0",
                    });
            AddOpenTelemetry(provider, env, resourceBuilder, endpoint)
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddOtlpExporter(opt =>
                    {
                        opt.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                        opt.Endpoint = new Uri(endpoint);
                    });
            });
            return provider;
        }
    }
}
