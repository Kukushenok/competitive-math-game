using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace RepositoriesTests
{
    public class IntegrationTest<T>(ITestOutputHelper helper) : ContainerInitializer where T: class
    {
        protected override async Task AfterDockerInit()
        {
            IServiceCollection coll = new ServiceCollection();
            var Dict = new Dictionary<string, string?>
            {
               {"ConnectionStrings:postgres", ConnectionString }
            };
            testDumper = new FileDumper(Path.Combine(CORE_PATH,"ResultDumps"));
            var conf = new ConfigurationBuilder().AddInMemoryCollection(Dict).Build();
            coll.AddSingleton<IConfiguration>(conf);
            coll.UseXUnitLogging(helper);
            coll.AddRepositories();
            ServiceProvider p = coll.BuildServiceProvider();
            Testing = p.GetService<T>()!;
            contextFactory = p.GetService<IDbContextFactory<BaseDbContext>>()!;
            Logger = p.GetService<ILogger<IntegrationTest<T>>>()!;
            Logger.LogInformation("Using connection string: " + ConnectionString);
        }
        protected T Testing;
        protected ILogger<IntegrationTest<T>> Logger;
        private FileDumper testDumper;
        private IDbContextFactory<BaseDbContext> contextFactory;

        public async Task<BaseDbContext> GetContext() => await contextFactory.CreateDbContextAsync();
        protected async Task ExecSQL(string sql)
        {
            var result = await Container.ExecScriptAsync(sql);
            result.Stderr.Should().BeEmpty("Execution error!");
        }
        protected async Task ExecSQLFile(string fileName)
        {
            string contents = await File.ReadAllTextAsync(Path.Combine(CORE_PATH, TEST_INIT_PATH, fileName));
            await ExecSQL(contents);
        }
        protected async Task DoDumpings(string dumpName)
        {
            List<string> commands = new List<string>() { "/bin/bash", "-c", $"PGPASSWORD={PASSWORD} pg_dump --username {USERNAME} {DATABASE}" };
            var result = await Container.ExecAsync(commands);
            await testDumper.Dump($"{typeof(T).Name}_{dumpName}", result.Stdout);
        }
    }
}
