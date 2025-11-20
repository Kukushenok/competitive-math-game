namespace RepositoriesRealisation.RewardGranters
{
    public interface IRewardGranter
    {
        Task GrantRewards(BaseDbContext context, int competitionID);
    }
}
