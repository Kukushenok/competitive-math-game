using CompetitiveBackend.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace RepositoriesRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection container)
        {
            container.AddScoped<IAccountRepository, AccountRepository>();
            container.AddScoped<ISessionRepository, SessionRepository>();
            container.AddScoped<IPlayerProfileRepository, PlayerProfileRepository>();
            container.AddScoped<SessionRepositoryConfiguration>();
            container.AddDbContextFactory<BaseDbContext, BaseContextFactory>();
            return container;
        }
    }
}
