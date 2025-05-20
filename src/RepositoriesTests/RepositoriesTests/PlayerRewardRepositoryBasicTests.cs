using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample")]
    public class PlayerRewardRepositoryBasicTests : IntegrationTest<IPlayerRewardRepository>
    {
        public PlayerRewardRepositoryBasicTests(ITestOutputHelper helper) : base(helper)
        {
        }
        [Fact]
        public async Task AddReward_Success()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await Testing.CreateReward(new PlayerReward(1, 1, "hi", "deo", null));
            using var context = await GetContext();
            context.PlayerReward.ToList().Should().ContainSingle().Which.Should().Satisfy((PlayerRewardModel x) =>
            {
                x.PlayerID.Should().Be(1);
                x.RewardDescriptionID.Should().Be(1);
            });
        }
        [Fact]
        public async Task AddReward_Failure()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await Assert.ThrowsAsync<FailedOperationException>(async () => await Testing.CreateReward(new PlayerReward(8, 1, "hi", "deo", null)));
        }
        [Fact]
        public async Task DeleteReward_Success()
        {
            await ExecSQLFile("mix_player_rewards.sql");
            await Testing.DeleteReward(1);
            using var context = await GetContext();
            context.PlayerReward.ToList().Should().NotContain(x => x.Id == 1);
        }
        [Fact]
        public async Task DeleteReward_Failure()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.DeleteReward(1));
        }
        public static object[][] limiters = [
                [new DataLimiter(0, 3)],
                [new DataLimiter(0, 2)],
                [new DataLimiter(0, 1)],
                [new DataLimiter(1, 1)],
                [new DataLimiter(2, 1)],
                [DataLimiter.NoLimit]
        ];
        [Theory]
        [MemberData(nameof(limiters))]
        public async Task GetPlayerRewards_Success(DataLimiter dt)
        {
            await ExecSQLFile("mix_player_rewards.sql");
            IEnumerable<PlayerReward> c = await Testing.GetAllRewardsOf(1, dt);
            int cnt = dt.Partition;
            if (dt.HasNoLimit) cnt = 3;
            c.ToList().Should().HaveCount(cnt);
        }
    }
}
