using BenchmarkMeasurerHost.DataGenerator;
using BenchmarkMeasurerHost.TimeMeasurer;
using Microsoft.Extensions.DependencyInjection;

namespace BenchmarkMeasurerHost
{
    public static class Installer
    {
        public static IServiceCollection AddBenchmarkTimeMeasurerHost(this IServiceCollection coll)
        {
            coll.AddScoped<ICompetitionEnvironmentGenerator, BaseDataGenerator>();
            coll.AddScoped<ITimeMeasurerHost, RewardExecuterHost>();
            return coll;
        }
    }
}
