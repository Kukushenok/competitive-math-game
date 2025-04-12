using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerParticipationWatchUseCase
    {
        public Task<PlayerParticipationDTO> GetParticipation(int competition, int accountID);
        public Task<IEnumerable<PlayerParticipationDTO>> GetLeaderboard(int competition, DataLimiterDTO limiter);
    }
}
