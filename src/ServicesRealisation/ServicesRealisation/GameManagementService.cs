using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class GameManagementService : IGameManagementService
    {
        private readonly IRiddleRepository _riddleRepository;
        private readonly IRiddleSettingsRepository _settingsRepository;

        public GameManagementService(
            IRiddleRepository riddleRepository,
            IRiddleSettingsRepository settingsRepository)
        {
            _riddleRepository = riddleRepository;
            _settingsRepository = settingsRepository;
        }

        public Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter)
        {
            return _riddleRepository.GetRiddles(competitionID, limiter);
        }

        public Task CreateRiddle(RiddleInfo riddle)
        {
            return _riddleRepository.CreateRiddle(riddle);
        }

        public Task UpdateRiddle(RiddleInfo riddle)
        {
            return _riddleRepository.UpdateRiddle(riddle);
        }

        public Task DeleteRiddle(int riddleID)
        {
            return _riddleRepository.DeleteRiddle(riddleID);
        }

        public Task<RiddleGameSettings> GetSettings(int competitionID)
        {
            return _settingsRepository.GetRiddleSettings(competitionID);
        }

        public Task UpdateSettings(int competitionID, RiddleGameSettings settings)
        {
            return _settingsRepository.UpdateRiddleSettings(competitionID, settings);
        }
    }
}
