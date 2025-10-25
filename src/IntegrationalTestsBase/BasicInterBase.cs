using System.Net.Http.Json;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Repositories.Repositories;
using RepositoriesRealisation;

namespace IntegrationalTests
{
    public class IntegrationalFixture : IDisposable
    {
        internal sealed class DummyConnectionStringGetter : IConnectionStringGetter
        {
            public string GetConnectionString()
            {
                return ConnectionString;
            }
        }

        protected const string COREPATH = "../../../";
        protected const string TESTINITPATH = "TestInitScripts";
        public HttpClient Client { get; }
        public BaseDbContext Context { get; }
        private const string ConnectionString = "Server=localhost;Port=5432;Userid=root;Password=postgres_password;Database=maindb";

        public IntegrationalFixture()
        {
            Client = new HttpClient { BaseAddress = new Uri("http://localhost:8080") };
            var services = new ServiceCollection();

            services.AddDbContextFactory<BaseDbContext, BaseContextFactory>(lifetime: ServiceLifetime.Scoped);
            services.AddSingleton<IConnectionStringGetter>(new DummyConnectionStringGetter());
            services.AddSingleton<ILoggerFactory>(new NullLoggerFactory());
            ServiceProvider s = services.BuildServiceProvider();
            Context = s.GetRequiredService<IDbContextFactory<BaseDbContext>>().CreateDbContext();
        }

        public async Task ExecSQL(string text)
        {
            await Context.Database.ExecuteSqlRawAsync(text);
        }

        public async Task ExecSQLFile(string fileName)
        {
            string contents = await File.ReadAllTextAsync(Path.Combine(COREPATH, TESTINITPATH, fileName));
            await ExecSQL(contents);
        }

        public void Dispose()
        {
            Client.Dispose();
            Context.Dispose();
        }
    }

    public class IntegrationalTest(IntegrationalFixture f) : IClassFixture<IntegrationalFixture>, IAsyncLifetime
    {
        public HttpClient Client => f.Client;
        public BaseDbContext Context => f.Context;
        public Faker Faker { get; private set; } = new Faker();
        private readonly List<object> rollback = [];
        public async Task DisposeAsync()
        {
            Context.RemoveRange(rollback);
            await Context.SaveChangesAsync();
        }

        public async Task InitializeAsync()
        {
            Faker = new Faker();
            await Init();
            await Context.SaveChangesAsync();
        }

        protected virtual Task Init()
        {
            return Task.CompletedTask;
        }

        protected async Task<T> Instantiate<T>(T model)
            where T : class
        {
            await Context.AddAsync(model);
            rollback.Add(model);
            return model;
        }

        protected Task ExecSQL(string text)
        {
            return f.ExecSQL(text);
        }

        protected Task ExecSQLFile(string fileName)
        {
            return f.ExecSQLFile(fileName);
        }
    }

    public static class HttpMessageExtensions
    {
        public static async Task<T> FromJSONAsync<T>(this HttpResponseMessage msg)
        {
            return (await msg.Content.ReadFromJsonAsync<T>())!;
        }
    }
}
