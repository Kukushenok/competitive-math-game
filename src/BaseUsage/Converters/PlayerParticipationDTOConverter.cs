using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.BaseUsage.Converters
{
    internal static class PlayerParticipationDTOConverter
    {
        public static PlayerParticipationDTO Convert(this PlayerParticipation participation)
        {
            return new PlayerParticipationDTO(participation.PlayerProfileId, participation.CompetitionId, participation.Score,
                participation.LastUpdateTime, participation.BindedProfile?.Convert(), participation.BindedCompetition?.Convert());
        }
        public static PlayerParticipation Convert(this PlayerParticipationDTO participation)
        {
            return new PlayerParticipation(participation.Competition, participation.AccountID, participation.Score, participation.LastUpdateTime);
        }
    }
}