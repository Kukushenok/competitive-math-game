namespace BenchmarkMeasurerHost.DataGenerator
{
    public class EnvironmentSettings
    {
        public int ParticipantsCount;
        public int SupposedRewardCount;
        public int RewardKindCount = 5;
        public EnvironmentSettings(int participantsCount, int supposedRewardCount, int rewardKindCount)
        {
            ParticipantsCount = participantsCount;
            SupposedRewardCount = supposedRewardCount;
            RewardKindCount = rewardKindCount;
        }
    }
}
