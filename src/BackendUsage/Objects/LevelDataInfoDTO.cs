using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class LevelDataInfoDTO : IntIdentifiableDTO
    {
        public int CompetitionID { get; set; }
        public string Platform { get; set; }
        public int VersionKey { get; set; }
        public LevelDataInfoDTO()
        {
            Platform = string.Empty;
            CompetitionID = 0;
            VersionKey = 0;
        }

        public LevelDataInfoDTO(int competitionID, string platform, int versionKey, int? id)
            : base(id)
        {
            CompetitionID = competitionID;
            Platform = platform;
            VersionKey = versionKey;
        }
    }
}
