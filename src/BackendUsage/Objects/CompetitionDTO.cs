namespace CompetitiveBackend.BackendUsage.Objects
{
    public record CompetitionDTO(int? ID, string? Name, string? Description, DateTime StartDate, DateTime EndDate): IntIdentifiableDTO(ID);
    public record CompetitionUpdateRequestDTO(int id, string? Name, string? Description, DateTime? StartDate, DateTime? EndDate) : IntIdentifiableDTO(id)
    {
        public CompetitionUpdateRequestDTO(int ID, CompetitionUpdateRequestDTO patch): this(patch)
        {
            this.ID = ID;
        }
    }
}
