using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage
{
    public class PlayerUseCase : IPlayerProfileUseCase
    {
        private IPlayerProfileService _playerProfileService;
        public PlayerUseCase(IPlayerProfileService playerProfileService)
        {
            _playerProfileService = playerProfileService;
        }

        public async Task<PlayerProfile> GetProfile(int id)
        {
            return await _playerProfileService.GetPlayerProfile(id);
        }

        public async Task<LargeData> GetProfileImage(int id)
        {
            return await _playerProfileService.GetPlayerProfileImage(id);
        }
    }
}
