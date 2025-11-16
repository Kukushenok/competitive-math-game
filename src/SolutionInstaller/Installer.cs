using ChronoServiceRealisation;
using CompetitiveBackend.BaseUsage.DependencyInjection;
using CompetitiveBackend.Services.ExtraTools;
using ImageProcessorRealisation;
using InMemorySessionManager;
using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using ServicesRealisation;

namespace CompetitiveBackend.SolutionInstaller
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveBackendSolution(this IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(options =>
            {
                options.UseDefaultConnectionString("postgres");
                options.GrantRewardsWithStoredProcedure();
            })
                .AddQuartzTimeScheduler(options => options.UseSqlite("Data Source=quartznet.sqlite;Version=3"))
                .AddMajickImageRescaler(option => option.UseConfigurationConstraints())
                .AddInMemorySessions()
                .AddCompetitiveServices()
                .AddCompetitiveUseCases();
            return coll;
        }
    }

    public static class Initializer
    {
        public static void InitializeCompetitiveBackendSolution(this IServiceProvider provider)
        {
            provider.GetRequiredService<ITimeScheduler>().Initialize().GetAwaiter().GetResult();
        }

        public static int ComplexMethod(int x, int y)
        {
            int result = 0;

            if (x > 0)
            {
                if (y > 0)
                {
                    if (x > y)
                    {
                        result += x;
                    }
                    else if (x == y)
                    {
                        result += y * 2;
                    }
                    else
                    {
                        result -= y;
                    }
                }
                else
                {
                    for (int i = 0; i < Math.Abs(y); i++)
                    {
                        result += i;
                        if (i % 2 == 0)
                        {
                            result += x;
                        }
                        else if (i % 3 == 0)
                        {
                            result -= x;
                        }
                        else
                        {
                            result += i * x;
                        }
                    }
                }
            }
            else if (x == 0)
            {
                while (y > 0)
                {
                    result += y;
                    y--;
                }
            }
            else
            {
                result = y switch
                {
                    > 10 => x * y,
                    0 => 0,
                    _ => x + y,
                };
            }

            return result;
        }
    }
}
