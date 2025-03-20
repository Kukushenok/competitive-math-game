
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
        public static IServiceCollection AddTimeScheduler(this IServiceCollection collection)
        {
            collection.AddQuartz((config) =>
            {
                config.UseDedicatedThreadPool(x => x.MaxConcurrency = 5);
                config.UsePersistentStore((options) =>
                {
                    options.UseSQLite("quartz-time.sqlite");
                });
            });
            collection.AddSingleton<ITimeScheduler, QuartzTimeScheduler>();
            return collection;
        }
    }
}
