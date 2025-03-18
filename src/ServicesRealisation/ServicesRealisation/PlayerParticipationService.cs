using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core;
using CompetitiveBackend.Repositories;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Repositories.Exceptions;

namespace CompetitiveBackend.Services.PlayerParticipationService
{
    public class PlayerParticipationService : IPlayerParticipationService
    {
        private readonly IPlayerParticipationRepository _playerParticipationRepository;
        public PlayerParticipationService(IPlayerParticipationRepository playerParticipationRepository)
        {
            _playerParticipationRepository = playerParticipationRepository;
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
            PlayerParticipation? participation = null;
            try
            {
                participation = await _playerParticipationRepository.GetParticipation(userID, competitionID);
            }
            catch(MissingDataException)
            {
                await _playerParticipationRepository.CreateParticipation(new PlayerParticipation(competitionID, userID, score));
            }
            if (participation != null && participation.Score < score)
            {
                await _playerParticipationRepository.UpdateParticipation(new PlayerParticipation(competitionID, userID, score));
            }
        }
    }

}
