namespace CompetitiveBackend.Core.Objects
{
    public class RewardDescription : IntIdentifiable
    {
        public string Name;
        public string Description;
        public RewardDescription(string name, string description, int? id = null) : base(id)
        {
            Name = name;
            Description = description;
        }
    }
}
