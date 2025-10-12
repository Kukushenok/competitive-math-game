using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RiddleGameSettingsDTO
    {
        public int ScoreOnRightAnswer { get; set; }
        public int ScoreOnBadAnswer { get; set; }
        public int TotalRiddles { get; set; }
        public TimeSpan? TimeLimit { get; set; }
        public int TimeLinearBonus { get; set; }

        public RiddleGameSettingsDTO(int scoreOnRightAnswer, int scoreOnBadAnswer, int totalRiddles, TimeSpan? timeLimit, int timeLinearBonus)
        {
            ScoreOnRightAnswer = scoreOnRightAnswer;
            ScoreOnBadAnswer = scoreOnBadAnswer;
            TotalRiddles = totalRiddles;
            TimeLimit = timeLimit;
            TimeLinearBonus = timeLinearBonus;
        }
    }
}
