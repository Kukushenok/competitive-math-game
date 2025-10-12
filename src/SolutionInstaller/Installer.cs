using CompetitiveBackend.BaseUsage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ImageProcessorRealisation;
using ServicesRealisation;
using RepositoriesRealisation;
using ChronoServiceRealisation;
using CompetitiveBackend.Repositories;
using Repositories.Repositories;
using CompetitiveBackend.Services.ExtraTools;
using InMemorySessionManager;

namespace CompetitiveBackend.SolutionInstaller
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveBackendSolution(this IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(options => { options.UsePrivilegiedConnectionString("Guest"); })
                .AddQuartzTimeScheduler(options => options.UseSqlite("Data Source=quartznet.sqlite;Version=3"))
                .AddMajickImageRescaler(option=>option.UseConfigurationConstraints())
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
