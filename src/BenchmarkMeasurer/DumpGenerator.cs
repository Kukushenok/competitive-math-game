using BenchmarkMeasurerHost.DataGenerator;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace BenchmarkMeasurer
{
    public class DumpGenerator : BenchmarkTest
    {
        public DumpGenerator(ITestOutputHelper helper)
            : base(helper)
        {
        }

        protected override string GetName()
        {
            return "Dumper";
        }

        [Theory]
        [MeasurementData(10)]
        public async Task MakeDumps(EnvironmentSettings settings)
        {
            using IServiceScope scope = serviceProvider.CreateScope();
            ICompetitionEnvironmentGenerator host = scope.ServiceProvider.GetRequiredService<ICompetitionEnvironmentGenerator>();
            CompetitiveBackend.Core.Objects.Competition stopwatch = await host.GenerateEnvironment(settings);
            await DoDumpings($"{settings.SupposedRewardCount}_{Guid.NewGuid()}");
        }
    }
}
