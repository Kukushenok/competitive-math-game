namespace CompetitiveBackend.Core.Objects
{
    public sealed class PlayerProfile : IntIdentifiable
    {
        public string Name;
        public string? Description;
        public PlayerProfile(string name, string? description = null, int? id = null)
            : base(id)
        {
            this.Name = name;
            Description = description;
        }
    }
}
