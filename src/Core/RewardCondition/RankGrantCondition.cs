namespace CompetitiveBackend.Core.RewardCondition
{
    public class RankGrantCondition : GrantCondition
    {
        public override string Type => "rank";
        public float MinRank;
        public float MaxRank;
        public RankGrantCondition(float minRank, float maxRank)
        {
            MinRank = minRank;
            MaxRank = maxRank;
        }
    }
}
