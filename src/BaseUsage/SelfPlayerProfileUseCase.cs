using CompetitiveBackend.BackendUsage;
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

        public async Task<LargeData> GetMyImage()
        {
            PlayerAuthCheck(out int id);
            return await _playerProfileService.GetPlayerProfileImage(id);
        }

        public async Task<PlayerProfile> GetMyProfile()
        {
            PlayerAuthCheck(out int id);
            return await _playerProfileService.GetPlayerProfile(id);
        }

        public async Task UpdateMyImage(LargeData data)
        {
            PlayerAuthCheck(out int id);
            await _playerProfileService.SetPlayerProfileImage(id, data);
        }

        public async Task UpdateMyPlayerProfile(PlayerProfile p)
        {
            PlayerAuthCheck(out int id);
            PlayerProfile updating = new PlayerProfile(p.Name, p.Description, id);
            await _playerProfileService.UpdatePlayerProfile(updating);
        }

        async Task<ISelfUseCase> IAuthableUseCase<ISelfUseCase>.Auth(string token) => await Auth(token);
    }
}
