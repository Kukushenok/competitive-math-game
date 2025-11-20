using ClientUsage.Client;
using ClientUsage.UseCases;
using CompetitiveBackend.BackendUsage.UseCases;
using Microsoft.Extensions.DependencyInjection;
namespace ClientUsage.Installer
{
    public static class Installer
    {
        public static IServiceCollection AddRemoteUseCases(this IServiceCollection coll, string address)
        {
            coll.AddSingleton<IHttpClient>(new HttpClientWrapper(new HttpClient() { BaseAddress = new Uri(address) }));
            coll.AddScoped<IAuthUseCase, AuthUseCase>();
            coll.AddScoped<ISelfUseCase, SelfUseCase>();
            coll.AddScoped<IPlayerProfileUseCase, PlayerProfileUseCase>();
            coll.AddScoped<ICompetitionWatchUseCase, CompetitionWatchUseCase>();
            coll.AddScoped<ICompetitionEditUseCase, CompetitionEditUseCase>();
            coll.AddScoped<IRewardDescriptionEditUseCase, RewardDescriptionEditUseCase>();
            coll.AddScoped<IRewardDescriptionWatchUseCase, RewardDescriptionWatchUseCase>();
            coll.AddScoped<IPlayerParticipationUseCase, PlayerParticipationUseCase>();
            coll.AddScoped<IPlayerParticipationWatchUseCase, PlayerParticipationWatchUseCase>();
            coll.AddScoped<ICompetitionRewardEditUseCase, CompetitionRewardEditUseCase>();
            coll.AddScoped<IPlayerRewardUseCase, PlayerRewardUseCase>();
            coll.AddScoped<IGamePlayUseCase, GamePlayUseCase>();
            coll.AddScoped<IGameManagementUseCase, GameManagementUseCase>();
            return coll;
        }
    }
}
