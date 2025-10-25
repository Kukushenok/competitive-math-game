using Testcontainers.PostgreSql;

namespace BenchmarkMeasurer
{
    public class ContainerInitializer : IAsyncLifetime
    {
        public const string COREPATH = "../../../";
        protected const string USERNAME = "tester";
        protected const string PASSWORD = "tester";
        protected const string DATABASE = "tester";
        protected const string TESTINITPATH = "TestInitScripts";
        protected const string BASEINITPATH = "../postgres/init";
        protected const string DUMMYINITPATH = "dummy";

        protected PostgreSqlContainer? container;
        protected string ConnectionString => container.GetConnectionString();

        public async Task DisposeAsync()
        {
            await container.StopAsync();
        }

        public async Task InitializeAsync()
        {
            container = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithResourceMapping(Path.Combine(COREPATH, CustomInitPath() ?? BASEINITPATH), "/docker-entrypoint-initdb.d/")
                .WithUsername(USERNAME)
                .WithPassword(PASSWORD)
                .WithDatabase(DATABASE)
                .Build();
            await container.StartAsync();
            await AfterDockerInit();
        }

        protected virtual Task AfterDockerInit()
        {
            return Task.CompletedTask;
        }

        protected virtual string? CustomInitPath()
        {
            return null;
        }
    }
}
