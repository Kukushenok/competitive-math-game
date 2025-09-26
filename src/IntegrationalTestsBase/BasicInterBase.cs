using Bogus;
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
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Repositories.Repositories;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using XUnitLoggingProvider;

namespace IntegrationalTests
{

    public class IntegrationalFixture : IDisposable
    {
        internal class DummyConnectionStringGetter : IConnectionStringGetter
        {
            public string GetConnectionString() => _connectionString;
        }
        protected const string CORE_PATH = "../../../";
        protected const string TEST_INIT_PATH = "TestInitScripts";
        public HttpClient Client { get; }
        public BaseDbContext Context { get; }
        private const string _connectionString = "Server=localhost;Port=5432;Userid=root;Password=postgres_password;Database=maindb";

        public IntegrationalFixture()
        {
            Client = new HttpClient { BaseAddress = new Uri("http://localhost:8080") };
            var services = new ServiceCollection();

            services.AddDbContextFactory<BaseDbContext, BaseContextFactory>(lifetime: ServiceLifetime.Scoped);
            services.AddSingleton<IConnectionStringGetter>(new DummyConnectionStringGetter());
            services.AddSingleton<ILoggerFactory>(new NullLoggerFactory());
            var s = services.BuildServiceProvider();
            Context = s.GetRequiredService<IDbContextFactory<BaseDbContext>>().CreateDbContext();
        }
        public async Task ExecSQL(string text)
        {
            await Context.Database.ExecuteSqlRawAsync(text);
        }
        public async Task ExecSQLFile(string fileName)
        {
            string contents = await File.ReadAllTextAsync(Path.Combine(CORE_PATH, TEST_INIT_PATH, fileName));
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
        public Faker Faker { get; private set; }
        private List<object> rollback = new List<object>();
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
        protected virtual Task Init() => Task.CompletedTask;
        protected async Task<T> Instantiate<T>(T model) where T : class
        {
            await Context.AddAsync(model);
            rollback.Add(model);
            return model;
        }

        protected Task ExecSQL(string text) => f.ExecSQL(text);
        protected Task ExecSQLFile(string fileName) => f.ExecSQLFile(fileName);
    }
    public static class HttpMessageExtensions {
        public static async Task<T> FromJSONAsync<T>(this HttpResponseMessage msg)
        {
            return (await msg.Content.ReadFromJsonAsync<T>())!;
        }

    }
}
