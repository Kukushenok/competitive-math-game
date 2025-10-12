using BenchmarkMeasurerHost.DataGenerator;
using BenchmarkMeasurerHost.TimeMeasurer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace BenchmarkMeasurer
{
    public class DumpGenerator : BenchmarkTest
    {
        public DumpGenerator(ITestOutputHelper helper) : base(helper)
        {
        }

        protected override string GetName() => "Dumper";
        [Theory]
        [MeasurementData(10)]
        public async Task MakeDumps(EnvironmentSettings settings)
        {
            using var scope = serviceProvider.CreateScope();
            ICompetitionEnvironmentGenerator host = scope.ServiceProvider.GetRequiredService<ICompetitionEnvironmentGenerator>();
            var stopwatch = await host.GenerateEnvironment(settings);
            await DoDumpings($"{settings.SupposedRewardCount}_{Guid.NewGuid()}");
        }

    }
}
