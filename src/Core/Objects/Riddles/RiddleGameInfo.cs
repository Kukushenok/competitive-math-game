namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class RiddleGameInfo
    {
        public int CompetitionID;
        public DateTime StartTime;
        public List<RiddleInfo> Riddles;
        public RiddleGameInfo(List<RiddleInfo> riddles, int competitionID, DateTime startTime)
        {
            Riddles = riddles;
            CompetitionID = competitionID;
            StartTime = startTime;
        }
    }

    public class CompetitionParticipationTask
    {
        public string SessionID;
        public List<UserRiddleInfo> Riddles;
        public CompetitionParticipationTask(string sessID, List<UserRiddleInfo> riddles)
        {
            SessionID = sessID;
            Riddles = riddles;
        }
    }
}
