namespace CompetitiveBackend.Core.Objects
{
    public sealed class PlayerProfile : IntIdentifiable
    {
        public string Name;
        public string? Description;
        public PlayerProfile(string Name, string? description = null, int? id = null) : base(id)
        {
            this.Name = Name;
            Description = description;
        }
    }
}
