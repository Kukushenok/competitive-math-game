namespace CompetitiveBackend.Core.Objects
{
    public class PlayerParticipation
    {
        public readonly int CompetitionId;
        public readonly int PlayerProfileId;
        public readonly PlayerProfile? BindedProfile;
        public readonly Competition? BindedCompetition;
        public int Score;
        public DateTime LastUpdateTime;

        // string PlayerDescription?
        public PlayerParticipation(int competitionId, int playerProfileId, int score, DateTime lastUpdateTime, PlayerProfile? profile = null, Competition? competition = null)
        {
            CompetitionId = competitionId;
            PlayerProfileId = playerProfileId;
            Score = score;
            BindedProfile = profile;
            BindedCompetition = competition;
            LastUpdateTime = lastUpdateTime;
        }
    }
}
