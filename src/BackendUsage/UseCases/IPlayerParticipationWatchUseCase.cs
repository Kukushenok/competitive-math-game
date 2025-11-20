using System.Collections.Generic;
using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerParticipationWatchUseCase
    {
        Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID);
        Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter);
    }
}
