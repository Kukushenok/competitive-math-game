namespace CompetitiveBackend.BackendUsage.Objects
{
    public record PlayerProfileDTO(string? Name, string? Description, int? ID): IntIdentifiableDTO(ID);
}
