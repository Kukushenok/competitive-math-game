using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class PlayerRewardDTO: IntIdentifiableDTO{
        public int PlayerID { get; set; }
        public int RewardDescriptionID  { get; set; }
        public int? GrantedCompetitionID { get; set; } = null;
        public DateTime? GrantDate { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
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
