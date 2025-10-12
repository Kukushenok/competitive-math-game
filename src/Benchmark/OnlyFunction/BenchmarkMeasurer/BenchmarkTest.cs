using BenchmarkMeasurer;
using BenchmarkMeasurerHost;
using BenchmarkMeasurerHost.DataGenerator;
using BenchmarkMeasurerHost.TimeMeasurer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace BenchmarkMeasurer
{
    public abstract class BenchmarkTest(ITestOutputHelper helper) : ContainerInitializer
    {
        protected static string ResultPath => Path.Combine(CORE_PATH, "Results");
        protected override Task AfterDockerInit()
        {
            IServiceCollection coll = new ServiceCollection();
            var Dict = new Dictionary<string, string?>
            {
               {"ConnectionStrings:postgres", ConnectionString}
            };

            var conf = new ConfigurationBuilder().AddInMemoryCollection(Dict).Build();
            coll.AddSingleton<IConfiguration>(conf);
            coll.UseXUnitLogging(helper);
            coll.AddBenchmarkTimeMeasurerHost();
            AddMyRepositories(coll);
            ServiceProvider p = coll.BuildServiceProvider();
            contextFactory = p.GetService<IDbContextFactory<BaseDbContext>>()!;
            serviceProvider = p;
            return Task.CompletedTask;
        }
        protected ServiceProvider serviceProvider;
        private FileDumper testDumper = new FileDumper(Path.Combine(CORE_PATH, "Results", "Dumps"));
        private IDbContextFactory<BaseDbContext> contextFactory = null!;
        
        public async Task<BaseDbContext> GetContext() => await contextFactory.CreateDbContextAsync();
        protected async Task ExecSQL(string sql)
        {
            var result = await Container.ExecScriptAsync(sql);
        }
        protected async Task ExecSQLFile(string fileName)
        {
            string contents = await File.ReadAllTextAsync(fileName);
            await ExecSQL(contents);
        }
        protected async Task DoDumpings(string dumpName, FileDumper? dumper = null)
        {
            dumper ??= testDumper;
            List<string> commands = new List<string>() { "/bin/bash", "-c", $"PGPASSWORD={PASSWORD} pg_dump --username {USERNAME} {DATABASE}" };
            var result = await Container.ExecAsync(commands);
            await dumper.Dump(dumpName, result.Stdout);
        }
        protected virtual void AddMyRepositories(IServiceCollection coll) => coll.AddCompetitiveRepositories();
        protected abstract string GetName();
        //[Theory]
        //[MeasurementData(10)]
        //public async Task DoTest(EnvironmentSettings settings)
        //{
        //    using var scope = serviceProvider.CreateScope();
        //    ITimeMeasurerHost host = scope.ServiceProvider.GetRequiredService<ITimeMeasurerHost>();
        //    var stopwatch = await host.Measure(settings);
        //    FileDumper sampleDumper = new FileDumper(Path.Combine(CORE_PATH, "Results", GetName()));
        //    await sampleDumper.Dump(settings.SupposedRewardCount.ToString(), $"{stopwatch.ElapsedMilliseconds}\n");
        //}
    }
}
