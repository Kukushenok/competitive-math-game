namespace CompetitiveBackend.BackendUsage.Objects
{
    public class RankRewardConditionDTO
    {
        public readonly float MinRank;
        public readonly float MaxRank;

        public RankRewardConditionDTO(float minRank, float maxRank)
        {
            MinRank = minRank;
            MaxRank = maxRank;
        }
    }

    public class PlaceRewardConditionDTO
    {
        public readonly int MinPlace;
        public readonly int MaxPlace;

        public PlaceRewardConditionDTO(int minPlace, int maxPlace)
        {
            MinPlace = minPlace;
            MaxPlace = maxPlace;
        }
    }

    public class UpdateCompetitionRewardDTO : IntIdentifiableDTO
    {
        public readonly int RewardDescriptionID;
        public readonly RankRewardConditionDTO? ConditionByRank;
        public readonly PlaceRewardConditionDTO? ConditionByPlace;

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

    public class CreateCompetitionRewardDTO : UpdateCompetitionRewardDTO
    {
        public readonly int CompetitionID;

        public CreateCompetitionRewardDTO(int? id, int rewardDescriptionID, int competitionID,
            RankRewardConditionDTO? conditionByRank = null,
            PlaceRewardConditionDTO? conditionByPlace = null)
            : base(id, rewardDescriptionID, conditionByRank, conditionByPlace)
        {
            CompetitionID = competitionID;
        }
    }

    public class CompetitionRewardDTO : CreateCompetitionRewardDTO
    {
        public readonly string? Name;
        public readonly string? Description;

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
