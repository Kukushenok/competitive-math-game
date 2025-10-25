using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace ChronoServiceRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddQuartzTimeScheduler(this IServiceCollection collection, Action<Options>? setup = null)
        {
            var opt = new Options();
            setup?.Invoke(opt);
            collection.AddQuartz((config) =>
            {
                config.UseDedicatedThreadPool(x => x.MaxConcurrency = 5);
                if (opt.InMemory)
                {
                    config.UseInMemoryStore();
                }
                else
                {
                    config.UsePersistentStore((options) =>
                    {
                        options.UseNewtonsoftJsonSerializer();
                        if (opt.SqliteConnectionString != null)
                        {
                            SQLiteEnsurance.Ensure(opt.SqliteConnectionString);
                            options.UseSQLite(opt.SqliteConnectionString);
                        }
                        else if (opt.PostgresConnectionString != null)
                        {
                            options.UsePostgres(opt.PostgresConnectionString);
                        }
                    });
                }
            });
            collection.AddSingleton<ITimeScheduler, QuartzTimeScheduler>();
            return collection;
        }
    }
}
