using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;

namespace CompetitiveBackend.BaseUsage.Converters
{
    public static class CompetitionRewardDTOConverter
    {
        public static GrantCondition GetCondition(this UpdateCompetitionRewardDTO rewardDTO)
        {
            GrantCondition cnd;
            if (rewardDTO.ConditionByPlace != null)
            {
                cnd = new PlaceGrantCondition(rewardDTO.ConditionByPlace.MinPlace, rewardDTO.ConditionByPlace.MaxPlace);
                if (rewardDTO.ConditionByRank != null) throw new InvalidInputDataException("Both conditions should not occur at the same time.");
            }
            else if (rewardDTO.ConditionByRank != null)
            {
                cnd = new RankGrantCondition(rewardDTO.ConditionByRank.MinRank, rewardDTO.ConditionByRank.MaxRank);
            }
            else
            {
                throw new InvalidInputDataException("Could not get supported reward condition");
            }
            return cnd;
        }
        public static CompetitionReward Convert(this CompetitionRewardDTO rewardDTO)
        {
            return new CompetitionReward(rewardDTO.RewardDescriptionID,
                    rewardDTO.CompetitionID, rewardDTO.Name ?? string.Empty, rewardDTO.Description ?? string.Empty,
                    rewardDTO.GetCondition(), rewardDTO.ID);
        }
        public static CompetitionRewardDTO Convert(this CompetitionReward reward)
        {
            PlaceRewardConditionDTO? placeDTO = null;
            RankRewardConditionDTO? rankDTO = null;
            if (reward.Condition is RankGrantCondition rankedCond) rankDTO = new RankRewardConditionDTO(rankedCond.minRank, rankedCond.maxRank);
            else if (reward.Condition is PlaceGrantCondition placedCond) placeDTO = new PlaceRewardConditionDTO(placedCond.minPlace, placedCond.maxPlace);
            return new CompetitionRewardDTO(reward.Id, reward.RewardDescriptionID, reward.CompetitionID, reward.Name, reward.Description) { ConditionByPlace = placeDTO, ConditionByRank = rankDTO };
        }
        public static CompetitionRewardDTO Extend(this CreateCompetitionRewardDTO dto)
        {
            return new CompetitionRewardDTO(dto.ID, dto.RewardDescriptionID, dto.CompetitionID, "", "", dto.ConditionByRank, dto.ConditionByPlace);
        }
    }
}
