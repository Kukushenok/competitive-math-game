using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class UserRiddleInfoDTO
    {
        public string Question { get; set; } = string.Empty;
        public RiddleAnswerDTO[] AvailableAnswers { get; set; } = Array.Empty<RiddleAnswerDTO>();

        public UserRiddleInfoDTO(string question, RiddleAnswerDTO[] availableAnswers)
        {
            Question = question;
            AvailableAnswers = availableAnswers;
        }
    }
}
