using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IGameManagementUseCase : IAuthableUseCase<IGameManagementUseCase>
    {
        Task<IEnumerable<RiddleInfoDTO>> GetRiddles(int competitionID, DataLimiterDTO limiter);
        Task CreateRiddle(RiddleInfoDTO riddle);
        Task UpdateRiddle(RiddleInfoDTO riddle);
        Task DeleteRiddle(int riddleID);
        Task<RiddleGameSettingsDTO> GetSettings(int competitionID);
        Task UpdateSettings(int competitionID, RiddleGameSettingsDTO settings);
    }
}
