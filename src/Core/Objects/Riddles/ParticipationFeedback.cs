namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class ParticipationFeedback
    {
        public int Score;
        public int RightAnswersCount;
        public int TotalAnswersCount;
        public ParticipationFeedback(int score, int rightAnswersCount, int totalAnswersCount)
        {
            Score = score;
            RightAnswersCount = rightAnswersCount;
            TotalAnswersCount = totalAnswersCount;
        }
    }
}
