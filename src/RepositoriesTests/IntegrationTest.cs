using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace RepositoriesTests
{
    public class IntegrationTest<T>(ITestOutputHelper helper) : ContainerInitializer
        where T : class
    {
        protected override Task AfterDockerInit()
        {
            IServiceCollection coll = new ServiceCollection();
            var dict = new Dictionary<string, string?>
            {
               { "ConnectionStrings:postgres", ConnectionString },
            };
            testDumper = new FileDumper(Path.Combine(COREPATH, "ResultDumps"));
            IConfigurationRoot conf = new ConfigurationBuilder().AddInMemoryCollection(dict).Build();
            coll.AddSingleton<IConfiguration>(conf);
            coll.UseXUnitLogging(helper);
            AddMyRepositories(coll);
            ServiceProvider p = coll.BuildServiceProvider();
            testing = p.GetRequiredService<T>();
            contextFactory = p.GetRequiredService<IDbContextFactory<BaseDbContext>>();
            logger = p.GetRequiredService<ILogger<IntegrationTest<T>>>();
            logger.LogInformation("Using connection string: " + ConnectionString);
            PostConfiguring(p);
            return Task.CompletedTask;
        }

        protected T testing = null!;
        protected ILogger<IntegrationTest<T>> logger = null!;
        private FileDumper testDumper = null!;
        private IDbContextFactory<BaseDbContext> contextFactory = null!;

        public async Task<BaseDbContext> GetContext()
        {
            return await contextFactory.CreateDbContextAsync();
        }

        protected async Task ExecSQL(string sql)
        {
            DotNet.Testcontainers.Containers.ExecResult result = await container.ExecScriptAsync(sql);
            result.Stderr.Should().BeEmpty("Execution error!");
        }

        protected async Task ExecSQLFile(string fileName)
        {
            string contents = await File.ReadAllTextAsync(Path.Combine(COREPATH, TESTINITPATH, fileName));
            await ExecSQL(contents);
        }

        protected async Task DoDumpings(string dumpName)
        {
            List<string> commands = ["/bin/bash", "-c", $"PGPASSWORD={PASSWORD} pg_dump --username {USERNAME} {DATABASE}"];
            DotNet.Testcontainers.Containers.ExecResult result = await container.ExecAsync(commands);
            await testDumper.Dump($"{typeof(T).Name}_{dumpName}", result.Stdout);
        }

        protected virtual void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories();
        }

        protected virtual void PostConfiguring(ServiceProvider p)
        {
        }
    }
}
