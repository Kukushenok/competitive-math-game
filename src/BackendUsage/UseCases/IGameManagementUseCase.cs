using CompetitiveBackend.BackendUsage.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IGameManagementUseCase: IAuthableUseCase<IGameManagementUseCase>
    {
        public Task<IEnumerable<RiddleInfoDTO>> GetRiddles(int competitionID, DataLimiterDTO limiter);
        public Task CreateRiddle(RiddleInfoDTO riddle);
        public Task UpdateRiddle(RiddleInfoDTO riddle);
        public Task DeleteRiddle(int riddleID);
        public Task<RiddleGameSettingsDTO> GetSettings(int competitionID);
        public Task UpdateSettings(int competitionID, RiddleGameSettingsDTO settings);
    }
}
