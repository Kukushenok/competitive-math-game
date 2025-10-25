using System;
using System.Collections.Generic;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RiddleInfoDTO : IntIdentifiableDTO
    {
        public int CompetitionID { get; set; }
        public string Question { get; set; } = string.Empty;
        public List<RiddleAnswerDTO> PossibleAnswers { get; set; } = new List<RiddleAnswerDTO>();
        public RiddleAnswerDTO TrueAnswer { get; set; }

        public RiddleInfoDTO(int competitionID, string question, List<RiddleAnswerDTO> possibleAnswers, RiddleAnswerDTO trueAnswer, int? iD = null)
            : base(iD)
        {
            CompetitionID = competitionID;
            Question = question;
            PossibleAnswers = possibleAnswers;
            TrueAnswer = trueAnswer;
        }
    }
}
