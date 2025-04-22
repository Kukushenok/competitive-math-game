using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class PlayerParticipationDTO
    {
        public int AccountID { get; set; }
        public int Competition { get; set; }
        public int Score { get; set; }
        public PlayerProfileDTO? ProfileInfo { get; set; } = null;
        public CompetitionDTO? CompetitionInfo { get; set; } = null;
        public PlayerParticipationDTO(int accountID, int competition, int score, PlayerProfileDTO? profileInfo = null, CompetitionDTO? competitionInfo = null)
        {
            AccountID = accountID;
            Competition = competition;
            Score = score;
            ProfileInfo = profileInfo;
            CompetitionInfo = competitionInfo;
        }
    }
}
