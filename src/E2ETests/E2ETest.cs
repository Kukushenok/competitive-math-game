using CompetitiveBackend;
using CompetitiveBackend.Controllers;
using CompetitiveBackend.Core.Objects;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace E2ETests
{
    public class E2ETest(ITestOutputHelper helper, WebApplicationFactory<Program> factory) : ContainerInitializer, IClassFixture<WebApplicationFactory<Program>>
    {
        public HttpClient Client { get; private set; }
        protected override Task AfterDockerInit()
        {
            factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Testing");
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddInMemoryCollection(new Dictionary<string, string?>
                        {
                            ["ConnectionStrings:postgres"] = ConnectionString,
                            ["ConnectionStrings:Player"] = ConnectionString,
                            ["ConnectionStrings:Admin"] = ConnectionString,
                            ["ConnectionStrings:Guest"] = ConnectionString,
                            ["ConnectionStrings:RewardScheduler"] = ConnectionString
                        });
                        
                    });
                    builder.ConfigureServices(x =>
                    {
                        x.UseXUnitLogging(helper);
                        x.AddControllers()
                            .AddApplicationPart(typeof(CompetitionController).Assembly);
                    });
                });
            testDumper = new FileDumper(Path.Combine(CORE_PATH, "ResultDumps"));
            Client = factory.CreateClient();

            factory.Server.Should().NotBeNull();
            return Task.CompletedTask;
        }
        protected ILogger Logger = null!;
        private FileDumper testDumper = null!;
        private IDbContextFactory<BaseDbContext> contextFactory = null!;

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
            await testDumper.Dump($"{dumpName}", result.Stdout);
        }
        public override async Task DisposeAsync()
        {
            //await factory.DisposeAsync();
            await base.DisposeAsync();
        }
        public async Task<T> GetObject<T>(HttpResponseMessage response)
        {
            var json = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<T>(json);
            obj.Should().NotBeNull();
            return obj!;
        }
        public async Task<Result> Post<In, Result>(string path, In obj)
        {
            var response = await Post(path, obj);
            response.Should().NotBeNull();
            return await GetObject<Result>(response);
        }
        public StringContent GetContent<T>(T obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
        public async Task<HttpResponseMessage> Post<In>(string path, In obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(path, content);
            return response;
        }
        public void SetAuth(string token)
        {
            // TODO: the WebServer should support K Authorization V Bearer XXXXX
            Client.DefaultRequestHeaders.Add("Bearer", token);
        }
    }
}
