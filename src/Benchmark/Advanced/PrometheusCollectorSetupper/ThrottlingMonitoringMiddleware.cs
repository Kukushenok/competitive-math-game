#if DISABLED
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
namespace PrometheusCollectorSetupper
{
    // In your middleware or controller
    public class ThrottlingMonitoringMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly SemaphoreSlim _concurrencySemaphore = new SemaphoreSlim(5000, 5000); // Example limit

        public ThrottlingMonitoringMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ThrottlingMetrics.CurrentConcurrentRequests.Inc();
            var stopwatch = Stopwatch.StartNew();

            try
            {
                // Check if we're at capacity
                if (!await _concurrencySemaphore.WaitAsync(0))
                {
                    ThrottlingMetrics.RequestThrottlingEvents.Inc();
                    context.Response.StatusCode = 503;
                    await context.Response.WriteAsync("Service temporarily overloaded");
                    return;
                }

                try
                {
                    await _next(context);
                }
                finally
                {
                    _concurrencySemaphore.Release();
                }
            }
            finally
            {
                stopwatch.Stop();
                ThrottlingMetrics.RequestDuration.Observe(stopwatch.Elapsed.TotalSeconds);
                ThrottlingMetrics.CurrentConcurrentRequests.Dec();
            }
        }
    }
}
#endif