using Microsoft.Extensions.Hosting;
using Prometheus;
using System.Diagnostics;
#if DISABLED
namespace PrometheusCollectorSetupper
{
    public class CpuPressureMonitor : BackgroundService
    {
        private readonly int _processorCount = Environment.ProcessorCount;

        // --- Metrics ---
        private static readonly Gauge CpuUsageRatio = Metrics.CreateGauge(
            "dotnet_cpu_usage_ratio",
            "CPU usage ratio for this process (0–1 per core)");


        private static readonly Histogram ThreadPoolWaitSeconds = Metrics.CreateHistogram(
            "dotnet_threadpool_wait_seconds",
            "Time to obtain a ThreadPool thread, used as a CPU starvation indicator.",
            new HistogramConfiguration
            {
                Buckets = Histogram.PowersOfTenDividedBuckets(1, 10, 10) // 0.1ms → 10s
            });
         
        // --- State ---
        private readonly TimeSpan _pollInterval = TimeSpan.FromSeconds(5);
        private TimeSpan _prevCpuTime;
        private DateTime _prevWallClock = DateTime.UtcNow;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Warm up
            _prevCpuTime = Process.GetCurrentProcess().TotalProcessorTime;

            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var currentCpuTime = Process.GetCurrentProcess().TotalProcessorTime;

                var cpuDelta = (currentCpuTime - _prevCpuTime).TotalSeconds;
                var wallDelta = (now - _prevWallClock).TotalSeconds;

                if (wallDelta > 0)
                {
                    // Ratio normalized to processor count (e.g. 1.0 = full utilization)
                    var usage = cpuDelta / wallDelta / _processorCount;
                    usage = Math.Clamp(usage, 0, 1);
                    CpuUsageRatio.Set(usage);
                }

                _prevCpuTime = currentCpuTime;
                _prevWallClock = now;

                // Schedule a thread to measure threadpool scheduling delay
                var sw = Stopwatch.StartNew();
                var tcs = new TaskCompletionSource();
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    sw.Stop();
                    ThreadPoolWaitSeconds.Observe(sw.Elapsed.TotalSeconds);
                    tcs.TrySetResult();
                });

                await tcs.Task;
                await Task.Delay(_pollInterval, stoppingToken);
            }
        }
    }

}
#endif