namespace CompetitiveBackend.Core.Objects
{
    public class PlayerParticipation
    {
        public readonly int CompetitionId;
        public readonly int PlayerProfileId;
        public int Score;
        public readonly PlayerProfile? BindedProfile = null;
        public readonly Competition? BindedCompetition = null;
        // string PlayerDescription?
        public PlayerParticipation(int competitionId, int playerProfileId, int score, PlayerProfile? profile = null, Competition? competition = null)
        {
            CompetitionId = competitionId;
            PlayerProfileId = playerProfileId;
            Score = score;
            BindedProfile = profile;
            BindedCompetition = competition;
        }
    }
}
