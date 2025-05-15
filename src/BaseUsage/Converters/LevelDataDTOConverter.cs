using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    static class LevelDataDTOConverter
    {
        public static LevelDataInfo Convert(this LevelDataInfoDTO dto)
        {
            return new LevelDataInfo(dto.CompetitionID, dto.VersionKey, dto.Platform, dto.ID);
        }
        public static LevelDataInfoDTO Convert(this LevelDataInfo info)
        {
            return new LevelDataInfoDTO(info.CompetitionID, info.PlatformName, info.VersionCode, info.Id);
        }
    }
}
