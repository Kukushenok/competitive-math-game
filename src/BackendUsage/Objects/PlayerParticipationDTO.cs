namespace CompetitiveBackend.BackendUsage.Objects
{
    public class PlayerParticipationDTO
    {
        public readonly int AccountID;
        public readonly int Competition;
        public readonly int Score;
        public readonly PlayerProfileDTO? ProfileInfo = null;
        public readonly CompetitionDTO? CompetitionInfo = null;
        public PlayerParticipationDTO(int accountID, int competition, int score, PlayerProfileDTO? profileInfo, CompetitionDTO? competitionInfo)
        {
            AccountID = accountID;
            Competition = competition;
            Score = score;
            ProfileInfo = profileInfo;
            CompetitionInfo = competitionInfo;
        }
    }
}
