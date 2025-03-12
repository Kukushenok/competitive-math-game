namespace CompetitiveBackend.Core.Objects
{
    public class PlayerParticipation
    {
        public readonly int CompetitionId;
        public readonly int PlayerProfileId;
        public int Score;
        public PlayerParticipation(int competitionId, int playerProfileId, int score)
        {
            CompetitionId = competitionId;
            PlayerProfileId = playerProfileId;
            Score = score;
        }
    }
}
