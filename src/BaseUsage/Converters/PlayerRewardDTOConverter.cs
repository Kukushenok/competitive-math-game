using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    public static class PlayerRewardDTOConverter
    {
        public static PlayerRewardDTO Convert(this PlayerReward reward)
        {
            return new PlayerRewardDTO(reward.PlayerID, reward.RewardDescriptionID, reward.CompetitionSource, reward.RewardDate, reward.Name, reward.Description, reward.Id);
        }

        public static PlayerReward Convert(this PlayerRewardDTO dto)
        {
            return new PlayerReward(dto.PlayerID, dto.RewardDescriptionID, dto.Name ?? string.Empty, dto.Description ?? string.Empty, dto.GrantedCompetitionID, dto.GrantDate, dto.ID);
        }
    }
}
