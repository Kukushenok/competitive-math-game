using BenchmarkMeasurerHost.DataGenerator;
using System.Diagnostics;

namespace BenchmarkMeasurerHost.TimeMeasurer
{
    public interface ITimeMeasurerHost
    {
        public Task<Stopwatch> Measure(EnvironmentSettings settings);
    }
}
