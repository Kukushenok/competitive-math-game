using System;
using System.Collections.Generic;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class CompetitionParticipationRequestDTO
    {
        public string SessionID { get; set; }
        public List<RiddleAnswerDTO> Answers { get; set; }

        public CompetitionParticipationRequestDTO(string sessionID, List<RiddleAnswerDTO> answers)
        {
            SessionID = sessionID;
            Answers = answers;
        }
    }
}
