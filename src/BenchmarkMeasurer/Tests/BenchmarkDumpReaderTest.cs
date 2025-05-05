using BenchmarkMeasurerHost.TimeMeasurer;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BenchmarkMeasurer.Tests
{
    [Collection("Sequential")]
    public abstract class BenchmarkDumpReaderTest: BenchmarkTest
    {
        protected BenchmarkDumpReaderTest(ITestOutputHelper helper) : base(helper)
        {
        }
        [Theory]
        [ClassData(typeof(Data)), Trait("Category", "Measurement")]
        public async Task DoTest(string dumpName)
        {
            await Init(); 
            using var scope = serviceProvider.CreateScope();
            await ExecSQLFile(dumpName);
            ITimeMeasurerHost host = scope.ServiceProvider.GetRequiredService<ITimeMeasurerHost>();
            var stopwatch = await host.Measure();
            string iterCount = Path.GetFileNameWithoutExtension(dumpName).Split('_')[0];
            FileDumper sampleDumper = new FileDumper(Path.Combine(CORE_PATH, "Results", GetName()));
            await sampleDumper.Dump(iterCount, $"{stopwatch.ElapsedMilliseconds}\n");
            await DoDumpings("RESULT_"+Path.GetFileNameWithoutExtension(dumpName), sampleDumper.Clone(false));
        }
        protected override string? CustomInitPath() => DUMMY_INIT_PATH;
        protected async Task Init()
        {
            using (var context = await GetContext())
            {
                context.AccountsProfileImages.Add(new RepositoriesRealisation.DatabaseObjects.AccountModelProfileImage());
            }
        }

    }
}
