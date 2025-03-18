namespace CompetitiveBackend.Core.Objects
{
    public class PlayerReward : RewardDescription
    {
        public readonly int PlayerID;
        public readonly int RewardDescriptionID;
        public int? CompetitionSource;
        public DateTime? RewardDate;
        public PlayerReward(int playerID, int rewardDescriptionID, string name, string description, int? competitionSource = null, DateTime? rewardDate = null, int? id = null) : base(name, description, id)
        {
            PlayerID = playerID;
            RewardDescriptionID = rewardDescriptionID;
            CompetitionSource = competitionSource;
            Name = name;
            Description = description;
            RewardDate = rewardDate;
        }
    }
}
