namespace CompetitiveBackend.Core.Objects
{
    public class CompetitionReward: RewardDescription
    {
        public readonly int RewardDescriptionID;
        public readonly int CompetitionID;
        public string ConditionName;
        public string ConditionDescription;
        public CompetitionReward(int rewardDescriptionID, int competitionID, string name, string description, string conditionName, string conditionDescription, int? id = null) : base(name, description, id)
        {
            RewardDescriptionID = rewardDescriptionID;
            CompetitionID = competitionID;
            ConditionName = conditionName;
            ConditionDescription = conditionDescription;
        }
    }
}
