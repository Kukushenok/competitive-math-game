using System;
using System.Collections.Generic;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class CompetitionParticipationRequestDTO
    {
        public int PlayerID { get; set; }
        public string SessionID { get; set; }
        public List<RiddleAnswerDTO> Answers { get; set; }

        public CompetitionParticipationRequestDTO(int playerID, string sessionID, List<RiddleAnswerDTO> answers)
        {
            PlayerID = playerID;
            SessionID = sessionID;
            Answers = answers;
        }
    }
}
