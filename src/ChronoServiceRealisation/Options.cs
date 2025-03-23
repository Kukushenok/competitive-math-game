using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoServiceRealisation
{
    public class Options
    {
        private IServiceCollection collection;
        public string PostgresConnectionString { get; private set; }
        public string SqliteConnectionString { get; private set; }
        public bool InMemory { get; set; } = true;
        public Options(IServiceCollection collection)
        {
            this.collection = collection;
            PostgresConnectionString = null!;
            SqliteConnectionString = null!;
        }

        public Options UsePostgres(string connectionString)
        {
            InMemory = false;
            PostgresConnectionString = connectionString;
            return this;
        }
        public Options UseSqlite(string sqliteConnectionString)
        {
            InMemory = false;
            SqliteConnectionString = sqliteConnectionString;
            return this;
        }
        public Options UseInMemoryDatabase()
        {
            InMemory = true;
            return this;
        }
    }
}
