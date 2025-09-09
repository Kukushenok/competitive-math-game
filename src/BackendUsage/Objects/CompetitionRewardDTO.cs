using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RankRewardConditionDTO
    {
        public float MinRank { get; set; }
        public float MaxRank { get; set; }

        public RankRewardConditionDTO(float minRank, float maxRank)
        {
            MinRank = minRank;
            MaxRank = maxRank;
        }
    }
    [Serializable]
    public class PlaceRewardConditionDTO
    {
        public int MinPlace { get; set; }
        public int MaxPlace { get; set; }

        public PlaceRewardConditionDTO(int minPlace, int maxPlace)
        {
            MinPlace = minPlace;
            MaxPlace = maxPlace;
        }
    }
    [Serializable]
    public class UpdateCompetitionRewardDTO : IntIdentifiableDTO
    {
        public int RewardDescriptionID { get; set; }
        public RankRewardConditionDTO? ConditionByRank { get; set; }
        public PlaceRewardConditionDTO? ConditionByPlace { get; set; }

        public UpdateCompetitionRewardDTO(int? id, int rewardDescriptionID,
            RankRewardConditionDTO? conditionByRank = null,
            PlaceRewardConditionDTO? conditionByPlace = null)
            : base(id)
        {
            RewardDescriptionID = rewardDescriptionID;
            ConditionByRank = conditionByRank;
            ConditionByPlace = conditionByPlace;
        }
    }
    [Serializable]
    public class CreateCompetitionRewardDTO : UpdateCompetitionRewardDTO
    {
        public int CompetitionID { get; set; }

        public CreateCompetitionRewardDTO(int? id, int rewardDescriptionID, int competitionID,
            RankRewardConditionDTO? conditionByRank = null,
            PlaceRewardConditionDTO? conditionByPlace = null)
            : base(id, rewardDescriptionID, conditionByRank, conditionByPlace)
        {
            CompetitionID = competitionID;
        }
    }
    [Serializable]
    public class CompetitionRewardDTO : CreateCompetitionRewardDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public CompetitionRewardDTO(int? id, int rewardDescriptionID, int competitionID,
            string? name = null, string? description = null,
            RankRewardConditionDTO? conditionByRank = null,
            PlaceRewardConditionDTO? conditionByPlace = null)
            : base(id, rewardDescriptionID, competitionID, conditionByRank, conditionByPlace)
        {
            Name = name;
            Description = description;
        }
    }
}
