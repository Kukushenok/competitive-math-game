using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class PlayerProfileDTO : IntIdentifiableDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PlayerProfileDTO(string? name, string? description, int? ID) : base(ID)
        {
            Name = name;
            Description = description;
        }
    }
}
