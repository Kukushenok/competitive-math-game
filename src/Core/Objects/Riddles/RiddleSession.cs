namespace CompetitiveBackend.Core.Objects.Riddles
{
    public class RiddleSession
    {
        public string SessionID;
        public RiddleGameInfo GameInfo;
        public DateTime ExpiresIn;
        public RiddleSession(string sessionID, RiddleGameInfo gameInfo, DateTime expiresIn)
        {
            SessionID = sessionID;
            GameInfo = gameInfo;
            ExpiresIn = expiresIn;
        }
    }
}
