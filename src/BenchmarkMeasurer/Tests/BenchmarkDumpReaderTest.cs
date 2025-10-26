using System.Diagnostics;
using BenchmarkMeasurerHost.TimeMeasurer;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace BenchmarkMeasurer.Tests
{
    [Collection("Sequential")]
    public abstract class BenchmarkDumpReaderTest : BenchmarkTest
    {
        protected BenchmarkDumpReaderTest(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Theory]
        [ClassData(typeof(Data))]
        [Trait("Category", "Measurement")]
        public async Task DoTest(string dumpName)
        {
            await Init();
            using IServiceScope scope = serviceProvider.CreateScope();
            await ExecSQLFile(dumpName);
            ITimeMeasurerHost host = scope.ServiceProvider.GetRequiredService<ITimeMeasurerHost>();
            Stopwatch stopwatch = await host.Measure();
            string iterCount = Path.GetFileNameWithoutExtension(dumpName).Split('_')[0];
            var sampleDumper = new FileDumper(Path.Combine(COREPATH, "Results", GetName()));
            await sampleDumper.Dump(iterCount, $"{stopwatch.ElapsedMilliseconds}\n");
            await DoDumpings("RESULT_" + Path.GetFileNameWithoutExtension(dumpName), sampleDumper.Clone(false));
        }

        protected override string? CustomInitPath()
        {
            return DUMMYINITPATH;
        }

        protected async Task Init()
        {
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
        }
    }
}
