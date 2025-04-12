using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage;
using Microsoft.Extensions.DependencyInjection;

namespace CompetitiveBackend.BaseUsage.DependencyInjection
{
    public static class Installer
    {
        public static IServiceCollection AddBaseUseCases(this IServiceCollection coll)
        {
            coll.AddScoped<IAuthUseCase, AuthUseCase>();
            coll.AddScoped<ISelfUseCase, SelfPlayerProfileUseCase>();
            coll.AddScoped<IPlayerProfileUseCase, PlayerUseCase>();
            coll.AddScoped<ICompetitionWatchUseCase, CompetitionWatchUseCase>();
            coll.AddScoped<ICompetitionEditUseCase, CompetitionEditUseCase>();
            return coll;
        }
    }
}
