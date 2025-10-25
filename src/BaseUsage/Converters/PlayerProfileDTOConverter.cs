using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class PlayerProfileDTOConverter
    {
        public static PlayerProfileDTO Convert(this PlayerProfile p)
        {
            return new PlayerProfileDTO(p.Name, p.Description, p.Id);
        }

        public static PlayerProfile Convert(this PlayerProfileDTO dto)
        {
            return new PlayerProfile(dto.Name ?? string.Empty, dto.Description, dto.ID);
        }
    }
}