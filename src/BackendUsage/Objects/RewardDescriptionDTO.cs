namespace CompetitiveBackend.BackendUsage.Objects
{
    public class RewardDescriptionDTO: IntIdentifiableDTO
    {
        public readonly string Name;
        public readonly string Description;
        public RewardDescriptionDTO(string name, string description, int? iD): base(iD)
        {
            Name = name;
            Description = description;
        }
    }
}
