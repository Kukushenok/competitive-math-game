
using CompetitiveBackend.Services.ExtraTools;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoServiceRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddTimeScheduler(this IServiceCollection collection, Action<Options>? setup = null)
        {
            Options opt = new Options(collection);
            setup?.Invoke(opt);
            collection.AddQuartz((config) =>
            {
                config.UseDedicatedThreadPool(x => x.MaxConcurrency = 5);
                if (opt.InMemory)
                    config.UseInMemoryStore();
                else
                {
                    config.UsePersistentStore((options) =>
                    {
                        if (opt.SqliteConnectionString != null)
                            options.UseSQLite(opt.SqliteConnectionString);
                        else if (opt.PostgresConnectionString != null)
                            options.UsePostgres(opt.PostgresConnectionString);
                        else
                            options.UseBinarySerializer();
                    });
                }
            });
            collection.AddSingleton<ITimeScheduler, QuartzTimeScheduler>();
            return collection;
        }
    }
}
