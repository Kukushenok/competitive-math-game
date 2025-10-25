using System;

namespace CompetitiveBackend.BackendUsage.Objects
{
    [Serializable]
    public class PlayerRewardDTO : IntIdentifiableDTO
    {
        public int PlayerID { get; set; }
        public int RewardDescriptionID { get; set; }
        public int? GrantedCompetitionID { get; set; }
        public DateTime? GrantDate { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public PlayerRewardDTO(int playerID, int rewardDID, int? gCID, DateTime? grdDate, string? name, string? description, int? iD)
            : base(iD)
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
