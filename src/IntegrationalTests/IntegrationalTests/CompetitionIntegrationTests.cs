using CompetitiveBackend.BackendUsage.Objects;
using FluentAssertions;
namespace IntegrationalTests
{
    public class CompetitionIntegrationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private RepositoriesRealisation.Models.CompetitionModel? tracking;

        [Fact]
        public async Task CompetitionFetchOne()
        {
            // Act
            HttpResponseMessage result = await Client.GetAsync($"/api/v1/competitions/{tracking.Id}"); // await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            CompetitionDTO obj = await result.FromJSONAsync<CompetitionDTO>();
            obj.ID.Should().Be(tracking.Id);
            obj.Name.Should().Be(tracking.Name);
        }

        [Fact]
        public async Task CompetitionFetchAll()
        {
            // Act
            HttpResponseMessage result = await Client.GetAsync("/api/v1/competitions/"); // await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            CompetitionDTO[] obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Length.Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task CompetitionFetchPage()
        {
            // Act
            HttpResponseMessage result = await Client.GetAsync("/api/v1/competitions/?page=1&count=1"); // await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            CompetitionDTO[] obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Should().ContainSingle();
        }

        protected override async Task Init()
        {
            tracking = await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("C", "D", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
        }
    }
}
