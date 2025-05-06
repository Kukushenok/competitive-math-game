using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using FluentAssertions;
using RepositoriesRealisation.Models;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using CompetitiveBackend.Repositories.Exceptions;

namespace RepositoriesTests.RepositoriesTests
{
    public class AlienGrantCondition: GrantCondition
    {
        public override string Type => "Not supported condition type :P";
    }
    public class CompetitionRewardRepositoryTests : IntegrationTest<ICompetitionRewardRepository>
    {
        public CompetitionRewardRepositoryTests(ITestOutputHelper helper) : base(helper)
        {
            
        }
        public static object[][] grantConditions = [
            [new RankGrantCondition(0, 1), 0],
            [new RankGrantCondition(0.5f, 1), 1],
            [new RankGrantCondition(0f, 0.5f), 2],
            [new PlaceGrantCondition(1, 2), 3],
            [new PlaceGrantCondition(2, 2), 4],
            [new PlaceGrantCondition(4, 50), 5]
            ];
        [Theory]
        [MemberData(nameof(grantConditions))]
        public async Task AddCompetitionReward_DifferentRanks(GrantCondition cond, int idx)
        {
            await ExecSQLFile("mix_no_rewards.sql");

            var reward = new CompetitionReward(1, 1, "Hi", "Deo", cond);
            await Testing.CreateCompetitionReward(reward);

            using var context = await GetContext();
            context.CompetitionReward.ToList().Should().ContainSingle().Which.Should().Satisfy((CompetitionRewardModel x) =>
            {
                x.CompetitionId.Should().Be(1);
                x.RewardDescriptionId.Should().Be(1);
                x.GetCondition().Should().BeEquivalentTo(cond);
            });
            await DoDumpings($"created_condition_{idx}");
        }
        [Fact]
        public async Task AddCompetitionReward_Alien()
        {
            await ExecSQLFile("mix_no_rewards.sql");
            var reward = new CompetitionReward(1, 1, "Hi", "Deo", new AlienGrantCondition());

            await Assert.ThrowsAsync<IncorrectOperationException>(async () => await Testing.CreateCompetitionReward(reward));
        }
        [Theory]
        [MemberData(nameof(grantConditions))]
        public async Task UpdateCompetitionReward(GrantCondition cond, int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var reward = new CompetitionReward(2, 2, "Hi", "Deo", cond, idx % 2 + 1);
            await Testing.UpdateCompetitionReward(reward);
            using var context = await GetContext();
            context.CompetitionReward.Find(reward.Id).Should().Satisfy((CompetitionRewardModel x) =>
            {
                x.CompetitionId.Should().Be(2);
                x.RewardDescriptionId.Should().Be(2);
                x.GetCondition().Should().BeEquivalentTo(cond);
            });
            await DoDumpings($"updated_condition_{idx}");
        }
        [Fact]
        public async Task UpdateCompetitionReward_Alien()
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var reward = new CompetitionReward(1, 1, "Hi", "Deo", new AlienGrantCondition(), 2);

            await Assert.ThrowsAsync<IncorrectOperationException>(async () => await Testing.UpdateCompetitionReward(reward));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task DeleteCompetitionReward_Success(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            await Testing.RemoveCompetitionReward(idx);
            using var context = await GetContext();
            context.CompetitionReward.ToList().Should().NotContain(x => x.Id == idx);
        }
        [Fact]
        public async Task DeleteCompetitionReward_Failure()
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            await Assert.ThrowsAsync<MissingDataException>(async () => await Testing.RemoveCompetitionReward(10));
        }
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task GetCompetitionReward_Success(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            CompetitionReward rwd = await Testing.GetCompetitionReward(idx);

            using var context = await GetContext();
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
        public async Task GetCompetitionRewards_Success(int idx)
        {
            await ExecSQLFile("mix_comp_rewards.sql");
            var lst = (await Testing.GetCompetitionRewards(idx)).ToList();
            int supposedLength = idx == 3 ? 0 : idx; // hardcoded for mix_comp_rewards.sql
            lst.Should().HaveCount(supposedLength);
        }
    }
}
