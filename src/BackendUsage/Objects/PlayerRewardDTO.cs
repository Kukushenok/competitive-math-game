using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    public class PlayerRewardDTO: IntIdentifiableDTO{
        public readonly int PlayerID;
        public readonly int RewardDescriptionID;
        public readonly int? GrantedCompetitionID = null;
        public readonly DateTime? GrantDate = null;
        public readonly string? Name = null;
        public readonly string? Description = null;
        public PlayerRewardDTO(int playerID, int rewardDID, int? gCID, DateTime? grdDate, string? name, string? description, int? ID): base(ID)
        {
            PlayerID = playerID;
            RewardDescriptionID = rewardDID;
            GrantedCompetitionID = gCID;
            GrantDate = grdDate;
            Name = name;
            Description = description;
        }
    }
}
