using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class LargeDataDTOConverter
    {
        public static LargeData Convert(this LargeDataDTO dto)
        {
            return new LargeData(dto.Data);
        }
        public static LargeDataDTO Convert(this LargeData data)
        {
            return new LargeDataDTO(data.Data ?? Array.Empty<byte>());
        }
    }
}