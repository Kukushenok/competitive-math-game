using CompetitiveBackend.Services;
using CompetitiveBackend.Services.AuthService;
using CompetitiveBackend.Services.CompetitionRewardService;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.PlayerParticipationService;
using CompetitiveBackend.Services.PlayerProfileService;
using CompetitiveBackend.Services.PlayerRewardService;
using Microsoft.Extensions.DependencyInjection;
using ServicesRealisation.ServicesRealisation;

namespace ServicesRealisation
{
    public static class ServicesInstaller
    {
        public static IServiceCollection AddServices(this IServiceCollection container)
        {
            container.AddAuthService();
            container.AddScoped<ICompetitionService, CompetitionService>();
            container.AddScoped<ICompetitionRewardService, CompetitionRewardService>();
            container.AddScoped<IPlayerParticipationService, PlayerParticipationService>();
            container.AddScoped<IPlayerProfileService, PlayerProfileService>();
            container.AddScoped<IPlayerRewardService, PlayerRewardService>();
            return container;
        }
        public static IServiceCollection AddAuthService(this IServiceCollection container)
        {
            container.AddScoped<IHashAlgorithm, SHA256HashAlgorithm>();
            container.AddScoped<IRoleCreator, BasicRoleCreator>();
            container.AddScoped<IAuthService, AuthService>();
            return container;
        }
    }
}
