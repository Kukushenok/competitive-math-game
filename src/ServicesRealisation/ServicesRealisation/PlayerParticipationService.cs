using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.PlayerParticipationService
{
    public class PlayerParticipationService : IPlayerParticipationService
    {
        private readonly IPlayerParticipationRepository playerParticipationRepository;
        public PlayerParticipationService(IPlayerParticipationRepository playerParticipationRepository)
        {
            this.playerParticipationRepository = playerParticipationRepository;
        }

        public async Task DeleteParticipation(int playerID, int competitionID)
        {
            await playerParticipationRepository.DeleteParticipation(playerID, competitionID);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter)
        {
            return await playerParticipationRepository.GetLeaderboard(competitionID, limiter);
        }

        public async Task<PlayerParticipation> GetParticipation(int playerID, int competitionID)
        {
            return await playerParticipationRepository.GetParticipation(playerID, competitionID, true, true);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int playerID, DataLimiter limiter)
        {
            return await playerParticipationRepository.GetPlayerParticipations(playerID, limiter);
        }
    }
}
