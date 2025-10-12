namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class RiddleGameSettings
    {
        public int ScoreOnRightAnswer;
        public int ScoreOnBadAnswer;
        public int TotalRiddles;
        public TimeSpan? TimeLimit;
        public int TimeLinearBonus;
        public RiddleGameSettings(int scoreOnRightAnswer, int scoreOnBadAnswer, int totalRiddles, TimeSpan? timeLimit, int timeLinearBonus)
        {
            ScoreOnRightAnswer = scoreOnRightAnswer;
            ScoreOnBadAnswer = scoreOnBadAnswer;
            TotalRiddles = totalRiddles;
            TimeLimit = timeLimit;
            TimeLinearBonus = timeLinearBonus;
        }
    }
}
