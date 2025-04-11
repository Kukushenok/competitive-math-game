using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;
using CompetitiveBackend.Services.AuthService;
using CompetitiveBackend.Services.CompetitionRewardService;
using CompetitiveBackend.Services.CompetitionService;
using CompetitiveBackend.Services.Objects;
using CompetitiveBackend.Services.PlayerParticipationService;
using CompetitiveBackend.Services.PlayerProfileService;
using CompetitiveBackend.Services.PlayerRewardService;
using Microsoft.Extensions.DependencyInjection;
using ServicesRealisation.Objects;
using ServicesRealisation.ServicesRealisation.Validator;

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
            container.AddScoped<ICompetitionRewardScheduler, CompetitionRewardScheduler>();
            return container;
        }
        public static IServiceCollection AddAuthService(this IServiceCollection container)
        {
            container.AddScoped<IHashAlgorithm, SHA256HashAlgorithm>();
            container.AddScoped<IRoleCreator, BasicRoleCreator>();
            container.AddScoped<IAuthService, AuthService>();
            container.AddValidators();
            return container;
        }
        public static IServiceCollection AddPlayerProfileService(this IServiceCollection container)
        {
            container.AddScoped<IPlayerProfileService, PlayerProfileService>();
            return container;
        }
        public static IServiceCollection AddServicesWhichAreDone(this IServiceCollection container)
        {
            container.AddValidators();
            container.AddAuthService();
            container.AddPlayerProfileService();
            return container;
        }
        public static IServiceCollection AddValidators(this IServiceCollection container)
        {
            container.AddScoped<IValidator<PlayerProfile>, PlayerAccountValidator>();
            container.AddScoped<IValidator<Account>, PlayerAccountValidator>();
            container.AddScoped<IValidator<AccountCreationData>, PlayerAccountValidator>();
            container.AddScoped<IValidator<Competition>, CompetitionValidator>();
            container.AddScoped<IValidator<RewardDescription>, RewardDescriptionValidator>();
            return container;
        }
    }
}
