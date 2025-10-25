using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class CompetitionDTOConverter
    {
        public static CompetitionDTO Convert(this Competition comp)
        {
            return new CompetitionDTO(comp.Id, comp.Name, comp.Description, comp.StartDate, comp.EndDate);
        }

        public static Competition Convert(this CompetitionDTO comp)
        {
            return new Competition(comp.Name ?? string.Empty, comp.Description ?? string.Empty, comp.StartDate, comp.EndDate, comp.ID);
        }
    }
}