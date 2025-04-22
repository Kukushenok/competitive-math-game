using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    public class CompetitionDTO : IntIdentifiableDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public CompetitionDTO(int? ID, string? name, string? description,
                             DateTime startDate, DateTime endDate)
            : base(ID)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }
    }

    public class CompetitionUpdateRequestDTO : IntIdentifiableDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public CompetitionUpdateRequestDTO(int id, string? name, string? description,
                                          DateTime? startDate, DateTime? endDate)
            : base(id)
        {
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
        }

        public CompetitionUpdateRequestDTO(int ID, CompetitionUpdateRequestDTO patch)
            : this(ID, patch.Name, patch.Description, patch.StartDate, patch.EndDate)
        {
        }
    }
}
