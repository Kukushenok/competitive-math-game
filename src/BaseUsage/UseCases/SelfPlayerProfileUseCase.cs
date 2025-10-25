using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class SelfPlayerProfileUseCase : BaseAuthableUseCase<SelfPlayerProfileUseCase>, ISelfUseCase
    {
        private readonly IPlayerProfileService playerProfileService;
        public SelfPlayerProfileUseCase(IAuthService authService, IPlayerProfileService playerProfileService)
            : base(authService)
        {
            this.playerProfileService = playerProfileService;
        }

        public async Task<LargeDataDTO> GetMyImage()
        {
            PlayerAuthCheck(out int id);
            return (await playerProfileService.GetPlayerProfileImage(id)).Convert();
        }

        public async Task<PlayerProfileDTO> GetMyProfile()
        {
            PlayerAuthCheck(out int id);
            return (await playerProfileService.GetPlayerProfile(id)).Convert();
        }

        public async Task UpdateMyImage(LargeDataDTO data)
        {
            PlayerAuthCheck(out int id);
            await playerProfileService.SetPlayerProfileImage(id, data.Convert());
        }

        public async Task UpdateMyPlayerProfile(PlayerProfileDTO p)
        {
            PlayerAuthCheck(out int id);
            var updating = new PlayerProfile(p.Name ?? string.Empty, p.Description, id);
            await playerProfileService.UpdatePlayerProfile(updating);
        }

        async Task<ISelfUseCase> IAuthableUseCase<ISelfUseCase>.Auth(string token)
        {
            return await Auth(token);
        }
    }
}
