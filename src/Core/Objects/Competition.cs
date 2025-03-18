namespace CompetitiveBackend.Core.Objects
{
    public class Competition : IntIdentifiable
    {
        public readonly string Name;
        public readonly string Description;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;
        public Competition(string name, string description, DateTime startDate, DateTime endDate, int? id = null) : base(id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
