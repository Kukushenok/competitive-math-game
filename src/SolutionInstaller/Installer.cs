using CompetitiveBackend.BaseUsage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using ImageProcessorRealisation;
using ServicesRealisation;
using RepositoriesRealisation;
using ChronoServiceRealisation;
using CompetitiveBackend.Repositories;
using Repositories.Repositories;

namespace CompetitiveBackend.SolutionInstaller
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveBackendSolution(this IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(options => { options.UsePrivilegiedConnectionString("Guest"); })
                .AddQuartzTimeScheduler()
                .AddMajickImageRescaler()
                .AddCompetitiveServices()
                .AddCompetitiveUseCases();
            return coll;
        }
    }
}
