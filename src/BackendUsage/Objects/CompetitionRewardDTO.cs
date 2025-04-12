namespace CompetitiveBackend.BackendUsage.Objects
{
    public record RankRewardConditionDTO(float MinRank, float MaxRank);
    public record PlaceRewardConditionDTO(int MinPlace, int MaxPlace);
    public record UpdateCompetitionRewardDTO(int? ID, int RewardDescriptionID, RankRewardConditionDTO? ConditionByRank = null, PlaceRewardConditionDTO? ConditionByPlace = null) : IntIdentifiableDTO(ID)
    {
    }
    public record CreateCompetitionRewardDTO(int? ID, int RewardDescriptionID, int CompetitionID, RankRewardConditionDTO? ConditionByRank = null, PlaceRewardConditionDTO? ConditionByPlace = null) :
        UpdateCompetitionRewardDTO(ID, RewardDescriptionID, ConditionByRank, ConditionByPlace);
    public record CompetitionRewardDTO(int? ID, int RewardDescriptionID, int CompetitionID, string? Name = null,
        string? Description = null, RankRewardConditionDTO? ConditionByRank = null, PlaceRewardConditionDTO? ConditionByPlace = null) 
        : CreateCompetitionRewardDTO(ID, RewardDescriptionID, CompetitionID, ConditionByRank, ConditionByPlace);
}
