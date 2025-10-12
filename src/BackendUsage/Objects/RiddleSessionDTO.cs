using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class RiddleSessionDTO
    {
        public string SessionID { get; set; }
        public RiddleGameInfoDTO GameInfo { get; set; }
        public DateTime ExpiresIn { get; set; }

        public RiddleSessionDTO(string sessionID, RiddleGameInfoDTO gameInfo, DateTime expiresIn)
        {
            SessionID = sessionID;
            GameInfo = gameInfo;
            ExpiresIn = expiresIn;
        }
    }
}
