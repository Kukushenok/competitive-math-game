using CompetitiveBackend.BaseUsage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ImageProcessorRealisation;
using ServicesRealisation;
using RepositoriesRealisation;
using ChronoServiceRealisation;

namespace CompetitiveBackend.SolutionInstaller
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveBackendSolution(this IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories()
                .AddQuartzTimeScheduler()
                .AddMajickImageRescaler()
                .AddCompetitiveServices()
                .AddCompetitiveUseCases();
            return coll;
        }
    }
}
