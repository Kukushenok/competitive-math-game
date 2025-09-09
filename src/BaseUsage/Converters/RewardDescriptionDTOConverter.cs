using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class RewardDescriptionDTOConverter
    {
        public static RewardDescription Convert(this RewardDescriptionDTO dto)
        {
            return new RewardDescription(dto.Name, dto.Description, dto.ID);
        }
        public static RewardDescriptionDTO Convert(this RewardDescription descr)
        {
            return new RewardDescriptionDTO(descr.Name, descr.Description, descr.Id);
        }
    }
}