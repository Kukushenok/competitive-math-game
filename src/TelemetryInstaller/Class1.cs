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
        public static IServiceCollection InstallTelemetry(this IServiceCollection provider, IHostEnvironment env)
        {
            ResourceBuilder resourceBuilder = ResourceBuilder.CreateDefault()
                    .AddService(serviceName: env.ApplicationName)
                    .AddAttributes(new Dictionary<string, object>
                    {
                        ["environment"] = env.EnvironmentName,
                        ["version"] = "1.0.0",
                    });
            provider.AddOpenTelemetry()
                .WithTracing(tracing =>
                {
                    tracing
                        .SetResourceBuilder(resourceBuilder)
                        .AddAspNetCoreInstrumentation(options =>
                        {
                            options.RecordException = true;
                            options.EnrichWithHttpRequest = (activity, request) =>
                            {
                                activity.SetTag("http.user_agent", request.Headers.UserAgent.ToString());
                            };
                        })
                        .AddHttpClientInstrumentation();

                    // Конфигурация экспортеров на основе среды
                    if (env.IsDevelopment())
                    {
                        tracing.AddConsoleExporter();
                    }

                    // Для CI/CD - экспорт в файл
                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri("http://localhost:4317");
                    });
                })
        .WithMetrics(metrics =>
        {
            metrics
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMeter("MonitoringDemo.Metrics")
                .AddConsoleExporter();
        });
            return provider;
        }
    }
}
