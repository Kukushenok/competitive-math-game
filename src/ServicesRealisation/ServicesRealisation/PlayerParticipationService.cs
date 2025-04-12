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
            return await _playerParticipationRepository.GetParticipation(userID, competitionID);
        }

        public async Task<IEnumerable<PlayerParticipation>> GetPlayerParticipations(int userID, DataLimiter limiter)
        {
            return await _playerParticipationRepository.GetPlayerParticipations(userID, limiter);
        }

        public async Task SubmitParticipation(int userID, int competitionID, int score)
        {
            PlayerParticipation? participation = null;
            Competition c = await _competitionRepository.GetCompetition(competitionID);
            DateTime current = DateTime.UtcNow;
            if (current < c.StartDate || current > c.EndDate)
            {
                throw new ChronologicalException("Could not participate; competition " + (current > c.EndDate ? "has ended" : "is not started yet"));
            }
            try
            {
                participation = await _playerParticipationRepository.GetParticipation(userID, competitionID);
            }
            catch (MissingDataException)
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
