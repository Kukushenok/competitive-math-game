using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Core.Objects
{
    public class LevelDataInfo: IntIdentifiable
    {
        public int CompetitionID;
        public readonly int VersionCode;
        public readonly string PlatformName;
        public LevelDataInfo(int competitionID, int version, string platform, int? ID = null) : base(ID)
        {
            CompetitionID = competitionID;
            VersionCode = version;
            PlatformName = platform;
        }
    }
}
