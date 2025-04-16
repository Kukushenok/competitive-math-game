namespace CompetitiveBackend.BackendUsage.Objects
{
    public class PlayerProfileDTO : IntIdentifiableDTO
    {
        public readonly string? Name;
        public readonly string? Description;
        public PlayerProfileDTO(string? name, string? description, int? ID) : base(ID)
        {
            Name = name;
            Description = description;
        }
    }
}
