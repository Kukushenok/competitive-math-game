using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.RewardCondition;
using FluentAssertions;
namespace IntegrationalTests
{
    public class CompetitionRewardsTest(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private RepositoriesRealisation.Models.CompetitionModel tracking;

        [Fact]
        public async Task GetRewards()
        {
            // Act
            var result = await Client.GetAsync($"/api/v1/competitions/{tracking.Id}/rewards/");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var r = await result.FromJSONAsync<CompetitionRewardDTO[]>();
            r.Should().HaveCount(2).And.AllSatisfy(x => x.Description.Should().NotBeNull());
        }
        [Fact]
        public async Task ParticipateNoLoggedIn()
        {
            // Act
            var result = await Client.GetAsync($"/api/v1/competitions/x/rewards/");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
        }
        protected override async Task Init()
        {
            tracking = await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            await Context.SaveChangesAsync();
            var Y = await Instantiate(new RepositoriesRealisation.Models.RewardDescriptionModel("Hello", "world"));
            await Context.SaveChangesAsync();
            var a = await Instantiate(new RepositoriesRealisation.Models.CompetitionRewardModel(Y.Id, tracking.Id, new RankGrantCondition(0.5f, 0.7f)));
            var b = await Instantiate(new RepositoriesRealisation.Models.CompetitionRewardModel(Y.Id, tracking.Id, new PlaceGrantCondition(1, 2)));
            await Context.SaveChangesAsync();
        }
    }
}
