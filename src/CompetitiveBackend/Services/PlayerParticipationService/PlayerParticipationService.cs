using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.PlayerParticipationService
{
    public class PlayerParticipationService : LogService<PlayerParticipationService>, IPlayerParticipationService
    {
        private readonly IPlayerParticipationRepository _playerParticipationRepository;
        private readonly IPlayerProfileRepository _playerProfileRepository;
        public PlayerParticipationService(ILogger<PlayerParticipationService> logger, IPlayerParticipationRepository playerParticipationRepository, IPlayerProfileRepository playerProfileRepository) : base(logger)
        {
            _playerParticipationRepository = playerParticipationRepository;
            _playerProfileRepository = playerProfileRepository;
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
            return await _playerParticipationRepository.GetParticipation(userID, competitionID);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int userID, DataLimiter limiter)
        {
            return await _playerParticipationRepository.GetPlayerParticipations(userID, limiter);
        }

        public async Task SubmitParticipation(int userID, int competitionID, int score)
        {
            await _playerParticipationRepository.UpdateParticipation(new PlayerParticipation(userID, competitionID, score));
        }
    }

}
