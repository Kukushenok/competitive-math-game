#if DISABLED
using Prometheus;
namespace PrometheusCollectorSetupper
{
    public class ThrottlingMetrics
    {
        public static readonly Counter RequestThrottlingEvents = Metrics
            .CreateCounter("app_request_throttling_events", "Number of request throttling events");

        public static readonly Counter DatabaseThrottlingEvents = Metrics
            .CreateCounter("app_database_throttling_events", "Number of database throttling events");

        public static readonly Gauge CurrentConcurrentRequests = Metrics
            .CreateGauge("app_current_concurrent_requests", "Current number of concurrent requests");

        public static readonly Histogram RequestDuration = Metrics
            .CreateHistogram("app_request_duration_seconds", "Request duration in seconds");

        public static readonly Counter MemoryAllocationFailures = Metrics
            .CreateCounter("app_memory_allocation_failures", "Memory allocation failures indicating memory pressure");
    }

}
#endif
