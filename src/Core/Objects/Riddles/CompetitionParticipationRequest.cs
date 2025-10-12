namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class CompetitionParticipationRequest
    {
        public int PlayerID;
        public string SessionID;
        public List<RiddleAnswer> Answers;
        public CompetitionParticipationRequest(int playerID, string sessionID, List<RiddleAnswer> answers)
        {
            PlayerID = playerID;
            SessionID = sessionID;
            Answers = answers;
        }
    }
}
