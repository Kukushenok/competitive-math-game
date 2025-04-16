using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    public class CompetitionDTO : IntIdentifiableDTO
    {
        public readonly string? Name;
        public readonly string? Description;
        public readonly DateTime StartDate;
        public readonly DateTime EndDate;

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
        public readonly string? Name;
        public readonly string? Description;
        public readonly DateTime? StartDate;
        public readonly DateTime? EndDate;

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
