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
    }
}
