using System;
using System.Collections.Generic;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class CompetitionParticipationTaskDTO
    {
        public string SessionID { get; set; }
        public List<UserRiddleInfoDTO> Riddles { get; set; }

        public CompetitionParticipationTaskDTO(string sessionID, List<UserRiddleInfoDTO> riddles)
        {
            SessionID = sessionID;
            Riddles = riddles;
        }
    }
}
