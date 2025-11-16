using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation.RepositoriesRealisation;

namespace RepositoriesRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveRepositories(this IServiceCollection container, Action<Options>? opt = null)
        {
            var myOptions = new Options(container);
            container.AddScoped<IAccountRepository, AccountRepository>();
            container.AddScoped<ISessionRepository, SessionRepository>();
            container.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
            container.AddScoped<IRewardDescriptionRepository, RewardDescriptionRepository>();
            container.AddScoped<ICompetitionRepository, CompetitionRepository>();
            container.AddScoped<ICompetitionRewardRepository, CompetitionRewardRepository>();

            // container.AddScoped<ICompetitionLevelRepository, CompetitionLevelDataRepository>();
            container.AddScoped<IPlayerRewardRepository, PlayerRewardRepository>();
            container.AddScoped<IPlayerParticipationRepository, PlayerParticipationRepository>();
            container.AddScoped<SessionRepositoryConfiguration>();
            container.AddScoped<IRiddleRepository, RiddleRepository>();
            container.AddScoped<IRiddleSettingsRepository, RiddleSettingsRepository>();
            container.AddDbContextFactory<BaseDbContext, BaseContextFactory>(lifetime: ServiceLifetime.Singleton);
            opt?.Invoke(myOptions);
            if (!myOptions.SetUpRewardGranter)
            {
                myOptions.GrantRewardsWithDefaultCalls();
            }

            if (!myOptions.SetUpConnectionStringGetter)
            {
                myOptions.UseDefaultConnectionString();
            }

            return container;
        }
    }
}
