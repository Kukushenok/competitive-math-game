using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.BaseUsage.Converters;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class GameManagementUseCase : BaseAuthableUseCase<GameManagementUseCase>, IGameManagementUseCase
    {
        private IGameManagementService managementService;
        public GameManagementUseCase(IAuthService authService, IGameManagementService managementService) : base(authService)
        {
            this.managementService = managementService;
        }

        public async Task CreateRiddle(RiddleInfoDTO riddle)
        {
            AdminAuthCheck(out _);
            await managementService.CreateRiddle(riddle.Convert());
        }

        public async Task DeleteRiddle(int riddleID)
        {
            AdminAuthCheck(out _);
            await managementService.DeleteRiddle(riddleID);
        }

        public async Task<IEnumerable<RiddleInfoDTO>> GetRiddles(int competitionID, DataLimiterDTO limiter)
        {
            AdminAuthCheck(out _);
            return from m in await managementService.GetRiddles(competitionID, limiter.Convert()) select m.Convert();
        }

        public async Task<RiddleGameSettingsDTO> GetSettings(int competitionID)
        {
            AdminAuthCheck(out _);
            return (await managementService.GetSettings(competitionID)).Convert();
        }

        public async Task UpdateRiddle(RiddleInfoDTO riddle)
        {
            AdminAuthCheck(out _);
            await managementService.UpdateRiddle(riddle.Convert());
        }

        public async Task UpdateSettings(int competitionID, RiddleGameSettingsDTO settings)
        {
            AdminAuthCheck(out _);
            await managementService.UpdateSettings(competitionID, settings.Convert());
        }

        async Task<IGameManagementUseCase> IAuthableUseCase<IGameManagementUseCase>.Auth(string token) => await Auth(token);
    }
}
