using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class ParticipationFeedbackDTO
    {
        public int Score { get; set; }
        public int RightAnswersCount { get; set; }
        public int TotalAnswersCount { get; set; }

        public ParticipationFeedbackDTO(int score, int rightAnswersCount, int totalAnswersCount)
        {
            Score = score;
            RightAnswersCount = rightAnswersCount;
            TotalAnswersCount = totalAnswersCount;
        }
    }
}
