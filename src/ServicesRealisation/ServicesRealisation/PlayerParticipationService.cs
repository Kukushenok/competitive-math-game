using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;

namespace CompetitiveBackend.Services.PlayerParticipationService
{
    public class PlayerParticipationService : IPlayerParticipationService
    {
        private readonly IPlayerParticipationRepository _playerParticipationRepository;
        private readonly ICompetitionRepository _competitionRepository;
        public PlayerParticipationService(IPlayerParticipationRepository playerParticipationRepository, ICompetitionRepository competitionRepository)
        {
            _playerParticipationRepository = playerParticipationRepository;
            _competitionRepository = competitionRepository;
        }

        public async Task DeleteParticipation(int playerID, int competitionID)
        {
            await _playerParticipationRepository.DeleteParticipation(playerID, competitionID);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetLeaderboard(int competitionID, DataLimiter limiter)
        {
            return await _playerParticipationRepository.GetLeaderboard(competitionID, limiter);
        }

        public async Task<PlayerParticipation> GetParticipation(int userID, int competitionID)
        {
            return await _playerParticipationRepository.GetParticipation(userID, competitionID, true, true);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int userID, DataLimiter limiter)
        {
            return await _playerParticipationRepository.GetPlayerParticipations(userID, limiter);
        }
    }

}
