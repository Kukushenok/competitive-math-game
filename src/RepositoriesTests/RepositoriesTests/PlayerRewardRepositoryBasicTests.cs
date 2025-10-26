using AwesomeAssertions;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    [Collection("sample")]
    public class PlayerRewardRepositoryBasicTests : IntegrationTest<IPlayerRewardRepository>
    {
        public PlayerRewardRepositoryBasicTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        [Fact]
        public async Task AddRewardSuccess()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await testing.CreateReward(new PlayerReward(1, 1, "hi", "deo", null));
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.PlayerReward.ToList().Should().ContainSingle().Which.Should().Satisfy((PlayerRewardModel x) =>
            {
                x.PlayerID.Should().Be(1);
                x.RewardDescriptionID.Should().Be(1);
            });
        }

        [Fact]
        public async Task AddRewardFailure()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await Assert.ThrowsAsync<FailedOperationException>(async () => await testing.CreateReward(new PlayerReward(8, 1, "hi", "deo", null)));
        }

        [Fact]
        public async Task DeleteRewardSuccess()
        {
            await ExecSQLFile("mix_player_rewards.sql");
            await testing.DeleteReward(1);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.PlayerReward.ToList().Should().NotContain(x => x.Id == 1);
        }

        [Fact]
        public async Task DeleteRewardFailure()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await testing.DeleteReward(1));
        }

        public static readonly object[][] Limiters = [
                [new DataLimiter(0, 3)],
                [new DataLimiter(0, 2)],
                [new DataLimiter(0, 1)],
                [new DataLimiter(1, 1)],
                [new DataLimiter(2, 1)],
                [DataLimiter.NoLimit]
        ];
        [Theory]
        [MemberData(nameof(Limiters))]
        public async Task GetPlayerRewardsSuccess(DataLimiter dt)
        {
            await ExecSQLFile("mix_player_rewards.sql");
            IEnumerable<PlayerReward> c = await testing.GetAllRewardsOf(1, dt);
            int cnt = dt.Partition;
            if (dt.HasNoLimit)
            {
                cnt = 3;
            }

            c.ToList().Should().HaveCount(cnt);
        }
    }
}
