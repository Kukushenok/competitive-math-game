using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RewardDescriptionDTO : IntIdentifiableDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RewardDescriptionDTO(string name, string description, int? iD)
            : base(iD)
        {
            Name = name;
            Description = description;
        }
    }
}
