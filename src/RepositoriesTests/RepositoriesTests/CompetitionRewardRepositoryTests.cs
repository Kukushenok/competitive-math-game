using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class AlienGrantCondition : GrantCondition
    {
        public override string Type => "Not supported condition type :P";
    }

    [Collection("sample")]
    public class CompetitionRewardRepositoryTests : IntegrationTest<ICompetitionRewardRepository>
    {
        public CompetitionRewardRepositoryTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        public static object[][] GrantConditions = [
            [new RankGrantCondition(0, 1), 0],
            [new RankGrantCondition(0.5f, 1), 1],
            [new RankGrantCondition(0f, 0.5f), 2],
            [new PlaceGrantCondition(1, 2), 3],
            [new PlaceGrantCondition(2, 2), 4],
            [new PlaceGrantCondition(4, 50), 5]
            ];
        [Theory]
        [MemberData(nameof(GrantConditions))]
        public async Task AddCompetitionRewardDifferentRanks(GrantCondition cond, int idx)
        {
            await ExecSQLFile("mix_no_rewards.sql");

            var reward = new CompetitionReward(1, 1, "Hi", "Deo", cond);
            await testing.CreateCompetitionReward(reward);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionReward.ToList().Should().ContainSingle().Which.Should().Satisfy((CompetitionRewardModel x) =>
            {
                x.CompetitionId.Should().Be(1);
                x.RewardDescriptionId.Should().Be(1);
                x.GetCondition().Should().BeEquivalentTo(cond);
            });
            await DoDumpings($"created_condition_{idx}");
        }

        [Fact]
        public async Task AddCompetitionRewardAlien()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            var reward = new CompetitionReward(1, 1, "Hi", "Deo", new AlienGrantCondition());

            await (async () => await testing.CreateCompetitionReward(reward)).Should().ThrowExactlyAsync<IncorrectOperationException>();
        }

        [Theory]
        [MemberData(nameof(GrantConditions))]
        public async Task UpdateCompetitionReward(GrantCondition cond, int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var reward = new CompetitionReward(2, 2, "Hi", "Deo", cond, (idx % 2) + 1);
            await testing.UpdateCompetitionReward(reward);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionReward.Find(reward.Id).Should().Satisfy((CompetitionRewardModel x) =>
            {
                x.CompetitionId.Should().Be(2);
                x.RewardDescriptionId.Should().Be(2);
                x.GetCondition().Should().BeEquivalentTo(cond);
            });
            await DoDumpings($"updated_condition_{idx}");
        }

        [Fact]
        public async Task UpdateCompetitionRewardAlien()
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var reward = new CompetitionReward(1, 1, "Hi", "Deo", new AlienGrantCondition(), 2);

            await (async () => await testing.UpdateCompetitionReward(reward)).Should().ThrowExactlyAsync<IncorrectOperationException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteCompetitionRewardSuccess(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            await testing.RemoveCompetitionReward(idx);
            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.CompetitionReward.ToList().Should().NotContain(x => x.Id == idx);
        }

        [Fact]
        public async Task DeleteCompetitionRewardFailure()
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            await (async () => await testing.RemoveCompetitionReward(10)).Should().ThrowExactlyAsync<MissingDataException>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCompetitionRewardSuccess(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            CompetitionReward rwd = await testing.GetCompetitionReward(idx);

            using RepositoriesRealisation.BaseDbContext context = await GetContext();
            context.RewardDescription.Find(rwd.RewardDescriptionID).Should().Satisfy((RewardDescriptionModel md) =>
            {
                rwd.Description.Should().Be(md.Description);
                rwd.Name.Should().Be(md.Name);
            });
            context.CompetitionReward.Find(rwd.Id).Should().Satisfy((CompetitionRewardModel x) =>
            {
                rwd.Id.Should().Be(x.Id);
                rwd.CompetitionID.Should().Be(x.CompetitionId);
                rwd.RewardDescriptionID.Should().Be(x.RewardDescriptionId);
            });
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetCompetitionRewardsSuccess(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var lst = (await testing.GetCompetitionRewards(idx)).ToList();
            int supposedLength = idx == 3 ? 0 : idx; // hardcoded for mix_comp_rewards.sql
            lst.Should().HaveCount(supposedLength);
        }
    }
}
