using CompetitiveBackend.Core.Objects.Riddles;
namespace CompetitiveBackend.Repositories
{
    public interface IRiddleSettingsRepository
    {
        public Task<RiddleGameSettings> GetRiddleSettings(int competitionID);
        public Task UpdateRiddleSettings(int competitionID, RiddleGameSettings settings);
    }
}
