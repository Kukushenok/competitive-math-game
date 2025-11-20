namespace CompetitiveBackend.Core.Objects
{
    public class Competition : IntIdentifiable, IEquatable<Competition>
    {
        public readonly string Name;
        public readonly string Description;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;
        public Competition(string name, string description, DateTime startDate, DateTime endDate, int? id = null)
            : base(id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public bool Equals(Competition? x)
        {
            return (x != null) &&
                (Name == x.Name) &&
                (Description == x.Description) &&
                (StartDate == x.StartDate) &&
                (EndDate == x.EndDate) &&
                (Id == x.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Competition);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, Description, StartDate, EndDate);
        }
    }
}
