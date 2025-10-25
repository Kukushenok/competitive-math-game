using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class PlayerUseCase : IPlayerProfileUseCase
    {
        private readonly IPlayerProfileService playerProfileService;
        public PlayerUseCase(IPlayerProfileService playerProfileService)
        {
            this.playerProfileService = playerProfileService;
        }

        public async Task<PlayerProfileDTO> GetProfile(int id)
        {
            return (await playerProfileService.GetPlayerProfile(id)).Convert();
        }

        public async Task<LargeDataDTO> GetProfileImage(int id)
        {
            return (await playerProfileService.GetPlayerProfileImage(id)).Convert();
        }
    }
}
