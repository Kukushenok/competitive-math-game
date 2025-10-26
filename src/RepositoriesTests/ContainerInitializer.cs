using Testcontainers.PostgreSql;

namespace RepositoriesTests
{
    public class ContainerInitializer : IAsyncLifetime
    {
        protected const string COREPATH = "../../../";
        protected const string USERNAME = "tester";
        protected const string PASSWORD = "tester";
        protected const string DATABASE = "tester";
        protected const string TESTINITPATH = "TestInitScripts";
        protected const string BASEINITPATH = "../postgres/init";

        protected PostgreSqlContainer container = null!;
        protected string ConnectionString => container.GetConnectionString();

        public async Task DisposeAsync()
        {
            await container.StopAsync();
        }

        public async Task InitializeAsync()
        {
            container = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithResourceMapping(Path.Combine(COREPATH, BASEINITPATH), "/docker-entrypoint-initdb.d/")
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
    }
}
