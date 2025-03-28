using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation.RepositoriesRealisation;

namespace RepositoriesRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection container)
        {
            container.AddScoped<IAccountRepository, AccountRepository>();
            container.AddScoped<ISessionRepository, SessionRepository>();
            container.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
            container.AddScoped<IRewardDescriptionRepository, RewardDescriptionRepository>();
            container.AddScoped<ICompetitionRepository, CompetitionRepository>();
            container.AddScoped<SessionRepositoryConfiguration>();
            container.AddDbContextFactory<BaseDbContext, BaseContextFactory>();
            return container;
        }
    }
}
