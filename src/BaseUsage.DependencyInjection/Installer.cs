using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace CompetitiveBackend.BaseUsage.DependencyInjection
{
    public static class Installer
    {
        public static IServiceCollection AddCompetitiveUseCases(this IServiceCollection coll)
        {
            coll.AddScoped<IAuthUseCase, AuthUseCase>();
            coll.AddScoped<ISelfUseCase, SelfPlayerProfileUseCase>();
            coll.AddScoped<IPlayerProfileUseCase, PlayerUseCase>();
            coll.AddScoped<ICompetitionWatchUseCase, CompetitionWatchUseCase>();
            coll.AddScoped<ICompetitionEditUseCase, CompetitionEditUseCase>();
            coll.AddScoped<IRewardDescriptionEditUseCase, RewardDescriptionEditUseCase>();
            coll.AddScoped<IRewardDescriptionWatchUseCase, RewardDescriptionWatchUseCase>();
            coll.AddScoped<IPlayerParticipationUseCase, PlayerParticipationUseCase>();
            coll.AddScoped<IPlayerParticipationWatchUseCase, PlayerParticipationWatchUseCase>();
            coll.AddScoped<ICompetitionRewardEditUseCase, CompetitionRewardEditUseCase>();
            coll.AddScoped<IPlayerRewardUseCase, PlayerRewardUseCase>();
            coll.AddScoped<ICompetitionLevelEditUseCase, CompetitionLevelEditUseCase>();
            coll.AddScoped<IGamePlayUseCase, GamePlayUseCase>();
            coll.AddScoped<IGameManagementUseCase, GameManagementUseCase>();
            return coll;
        }
    }
}
