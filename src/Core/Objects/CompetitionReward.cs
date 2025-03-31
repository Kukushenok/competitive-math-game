using Core.RewardCondition;

namespace CompetitiveBackend.Core.Objects
{
    public class CompetitionReward : RewardDescription
    {
        public readonly int RewardDescriptionID;
        public readonly int CompetitionID;
        public GrantCondition Condition;
        public CompetitionReward(int rewardDescriptionID, int competitionID, string name, string description, GrantCondition condition, int? id = null) : base(name, description, id)
        {
            RewardDescriptionID = rewardDescriptionID;
            CompetitionID = competitionID;
            Condition = condition;
        }
    }
}
