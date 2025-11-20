namespace ChronoServiceRealisation
{
    public class Options
    {
        public string PostgresConnectionString { get; private set; }
        public string SqliteConnectionString { get; private set; }
        public bool InMemory { get; set; } = true;
        public Options()
        {
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
