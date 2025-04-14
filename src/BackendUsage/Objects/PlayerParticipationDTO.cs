namespace CompetitiveBackend.BackendUsage.Objects
{
    public record PlayerParticipationDTO(int AccountID, int Competition, int Score, PlayerProfileDTO? ProfileInfo = null, CompetitionDTO? CompetitionInfo = null);
}
