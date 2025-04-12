using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
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

        public async Task<PlayerProfileDTO> GetProfile(int id)
        {
            return (await _playerProfileService.GetPlayerProfile(id)).Convert();
        }

        public async Task<LargeDataDTO> GetProfileImage(int id)
        {
            return (await _playerProfileService.GetPlayerProfileImage(id)).Convert();
        }
    }
}
