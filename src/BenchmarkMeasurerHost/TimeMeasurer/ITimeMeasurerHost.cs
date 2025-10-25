using System.Diagnostics;
using BenchmarkMeasurerHost.DataGenerator;

namespace BenchmarkMeasurerHost.TimeMeasurer
{
    public interface ITimeMeasurerHost
    {
        Task<Stopwatch> Measure(EnvironmentSettings settings);
        Task<Stopwatch> Measure();
    }
}
