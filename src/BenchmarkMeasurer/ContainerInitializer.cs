using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testcontainers.PostgreSql;

namespace BenchmarkMeasurer
{
    public class ContainerInitializer: IAsyncLifetime
    {
        public const string CORE_PATH = "../../../";
        protected const string USERNAME = "tester";
        protected const string PASSWORD = "tester";
        protected const string DATABASE = "tester";
        protected const string TEST_INIT_PATH = "TestInitScripts";
        protected const string BASE_INIT_PATH = "BaseInitScript";
        protected const string DUMMY_INIT_PATH = "dummy";

        protected PostgreSqlContainer Container;
        protected string ConnectionString => Container.GetConnectionString();

        public async Task DisposeAsync()
        {
            await Container.StopAsync();
        }

        public async Task InitializeAsync()
        {
            Container = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithResourceMapping(Path.Combine(CORE_PATH, CustomInitPath() ?? BASE_INIT_PATH), "/docker-entrypoint-initdb.d/")
                .WithUsername(USERNAME)
                .WithPassword(PASSWORD)
                .WithDatabase(DATABASE)
                .Build();
            await Container.StartAsync();
            await AfterDockerInit();
        }

        protected virtual Task AfterDockerInit() => Task.CompletedTask;
        protected virtual string? CustomInitPath() => null;
    }
}
