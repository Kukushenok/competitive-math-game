using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.Objects.Riddles;
using CompetitiveBackend.Repositories;

namespace CompetitiveBackend.Services.RewardDescriptionService
{
    public class GameManagementService : IGameManagementService
    {
        private readonly IRiddleRepository riddleRepository;
        private readonly IRiddleSettingsRepository settingsRepository;

        public GameManagementService(
            IRiddleRepository riddleRepository,
            IRiddleSettingsRepository settingsRepository)
        {
            this.riddleRepository = riddleRepository;
            this.settingsRepository = settingsRepository;
        }

        public Task<IEnumerable<RiddleInfo>> GetRiddles(int competitionID, DataLimiter limiter)
        {
            return riddleRepository.GetRiddles(competitionID, limiter);
        }

        public Task CreateRiddle(RiddleInfo riddle)
        {
            return riddleRepository.CreateRiddle(riddle);
        }

        public Task UpdateRiddle(RiddleInfo riddle)
        {
            return riddleRepository.UpdateRiddle(riddle);
        }

        public Task DeleteRiddle(int riddleID)
        {
            return riddleRepository.DeleteRiddle(riddleID);
        }

        public Task<RiddleGameSettings> GetSettings(int competitionID)
        {
            return settingsRepository.GetRiddleSettings(competitionID);
        }

        public Task UpdateSettings(int competitionID, RiddleGameSettings settings)
        {
            return settingsRepository.UpdateRiddleSettings(competitionID, settings);
        }
    }
}
