namespace CompetitiveBackend.BackendUsage.Objects
{
    public record RewardDescriptionDTO(string Name, string Description, int? ID): IntIdentifiableDTO(ID);
}
