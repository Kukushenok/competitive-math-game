using System;
using System.Collections.Generic;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RiddleGameInfoDTO
    {
        public int CompetitionID { get; set; }
        public DateTime StartTime { get; set; }
        public List<RiddleInfoDTO> Riddles { get; set; }

        public RiddleGameInfoDTO(List<RiddleInfoDTO> riddles, int competitionID, DateTime startTime)
        {
            Riddles = riddles;
            CompetitionID = competitionID;
            StartTime = startTime;
        }
    }
}
