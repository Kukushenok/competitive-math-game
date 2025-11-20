namespace CompetitiveBackend.Core.Objects
{
    [Obsolete("We're determined not to do this")]
    public class LevelDataInfo : IntIdentifiable
    {
        public readonly int VersionCode;
        public readonly string PlatformName;
        public int CompetitionID;
        public LevelDataInfo(int competitionID, int version, string platform, int? iD = null)
            : base(iD)
        {
            CompetitionID = competitionID;
            VersionCode = version;
            PlatformName = platform;
        }
    }
}
