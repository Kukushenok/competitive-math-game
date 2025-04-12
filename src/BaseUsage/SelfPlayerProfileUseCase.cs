using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage
{
    public class SelfPlayerProfileUseCase : BaseAuthableUseCase<SelfPlayerProfileUseCase>, ISelfUseCase
    {
        private IPlayerProfileService _playerProfileService;
        public SelfPlayerProfileUseCase(IAuthService authService, IPlayerProfileService playerProfileService) : base(authService)
        {
            this._playerProfileService = playerProfileService;
        }

        public async Task<LargeDataDTO> GetMyImage()
        {
            PlayerAuthCheck(out int id);
            return (await _playerProfileService.GetPlayerProfileImage(id)).Convert();
        }

        public async Task<PlayerProfileDTO> GetMyProfile()
        {
            PlayerAuthCheck(out int id);
            return (await _playerProfileService.GetPlayerProfile(id)).Convert();
        }

        public async Task UpdateMyImage(LargeDataDTO data)
        {
            PlayerAuthCheck(out int id);
            await _playerProfileService.SetPlayerProfileImage(id, data.Convert());
        }

        public async Task UpdateMyPlayerProfile(PlayerProfileDTO p)
        {
            PlayerAuthCheck(out int id);
            PlayerProfile updating = new PlayerProfile(p.Name ?? string.Empty, p.Description, id);
            await _playerProfileService.UpdatePlayerProfile(updating);
        }

        async Task<ISelfUseCase> IAuthableUseCase<ISelfUseCase>.Auth(string token) => await Auth(token);
    }
}
