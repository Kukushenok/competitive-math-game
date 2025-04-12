namespace CompetitiveBackend.BackendUsage.Objects
{
    public record PlayerRewardDTO(int PlayerID, int RewardDescriptionID, int? GrantedCompetitionID = null,
        DateTime? GrantDate = null, string? Name = null, string? Description = null, int? ID = null): IntIdentifiableDTO(ID); 
}
