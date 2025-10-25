using CompetitiveBackend.Core.Objects.Riddles;
namespace CompetitiveBackend.Repositories
{
    public interface IRiddleSettingsRepository
    {
        Task<RiddleGameSettings> GetRiddleSettings(int competitionID);
        Task UpdateRiddleSettings(int competitionID, RiddleGameSettings settings);
    }
}
