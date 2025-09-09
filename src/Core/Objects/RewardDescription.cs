namespace CompetitiveBackend.Core.Objects
{
    public class RewardDescription : IntIdentifiable, IEquatable<RewardDescription>
    {
        public string Name;
        public string Description;
        public RewardDescription(string name, string description, int? id = null) : base(id)
        {
            Name = name;
            Description = description;
        }

        public bool Equals(RewardDescription? other)
        {
            return other != null && Id == other.Id && Name == other.Name && Description == other.Description;
        }
    }
}
