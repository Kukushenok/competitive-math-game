using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation.RepositoriesRealisation;

namespace RepositoriesRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection container, Action<Options>? opt = null)
        {
            Options myOptions = new Options(container);
            container.AddScoped<IAccountRepository, AccountRepository>();
            container.AddScoped<ISessionRepository, SessionRepository>();
            container.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
            container.AddScoped<IRewardDescriptionRepository, RewardDescriptionRepository>();
            container.AddScoped<ICompetitionRepository, CompetitionRepository>();
            container.AddScoped<ICompetitionRewardRepository, CompetitionRewardRepository>();
            container.AddScoped<IPlayerRewardRepository, PlayerRewardRepository>();
            container.AddScoped<SessionRepositoryConfiguration>();
            container.AddDbContextFactory<BaseDbContext, BaseContextFactory>();
            opt?.Invoke(myOptions);
            if (!myOptions.SetUpRewardGranter) myOptions.GrantRewardsWithDefaultCalls();
            if (!myOptions.SetUpConnectionStringGetter) myOptions.UseDefaultConnectionString();
            return container;
        }
    }
}
