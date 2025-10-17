using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;
using Prometheus.SystemMetrics;

namespace PrometheusCollectorSetupper
{
    public static class PrometheusSetupper
    {
        public static IServiceCollection AddPrometheusMetrics(this IServiceCollection services)
        {
            services.UseHttpClientMetrics();
            services.AddSystemMetrics();
            services.AddHostedService<CpuPressureMonitor>();
            return services;
        }

        // Новый метод для настройки middleware
        public static WebApplication UsePrometheusMetrics(this WebApplication app)
        {
            // Включаем сбор метрик HTTP запросов
            app.UseMetricServer();
            app.UseHttpMetrics();
            app.UseMiddleware<ThrottlingMonitoringMiddleware>();
            return app;
        }
    }
}
