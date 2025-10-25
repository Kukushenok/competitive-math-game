using System.Net.Http.Json;
using Allure.Xunit.Attributes.Steps;
using Bogus;
using CompetitiveBackend.BackendUsage.Objects;
using FluentAssertions;
using IntegrationalTests;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.Models;
namespace E2ETests
{
    public class CreateAccountThenParticipateE2ETest : IntegrationalTest
    {
        public CreateAccountThenParticipateE2ETest(IntegrationalFixture f)
            : base(f)
        {
        }

        [Fact]
        public async Task CompetitionFetchOne()
        {
            AuthSuccessResultDTO acc = await CreateAccount();
            CompetitionDTO comp = await GetCompetition();
            await Participate(comp);
            await GetLeaderboard(comp, acc);

            await RemoveAccount(acc);
        }

        [AllureStep("Create account")]
        private async Task<AuthSuccessResultDTO> CreateAccount()
        {
            HttpResponseMessage x = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(
                Faker.Internet.UserName(),
                "MySuperLongPassword",
                "mrcoolmoder@gmail.com"));
            AuthSuccessResultDTO result = await x.FromJSONAsync<AuthSuccessResultDTO>();
            Client.DefaultRequestHeaders.Add("Bearer", result.Token);
            return result;
        }

        [AllureStep("Cleanup")]
        private async Task RemoveAccount(AuthSuccessResultDTO dto)
        {
            int idx = dto.AccountID;
            await Context.PlayerParticipation.Where(x => x.AccountID == idx).ExecuteDeleteAsync();
            List<AccountModel> accs = await Context.AccountsReadOnly.Where(x => x.Id == idx).ToListAsync();
            Context.AccountsReadOnly.RemoveRange(accs);
            await Context.SaveChangesAsync();
        }

        [AllureStep("Get competition")]
        private async Task<CompetitionDTO> GetCompetition()
        {
            HttpResponseMessage result = await Client.GetAsync("/api/v1/competitions/?count=1");
            result.IsSuccessStatusCode.Should().BeTrue();
            CompetitionDTO[] obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Should().ContainSingle();
            return obj[0];
        }

        [AllureStep("Participate")]
        private async Task Participate(CompetitionDTO dto)
        {
            HttpResponseMessage x = await Client.PutAsync($"/api/v1/competitions/{dto.ID}/participations/?score=500", null);
            x.IsSuccessStatusCode.Should().BeTrue();
        }

        [AllureStep("Get leaderboard")]
        private async Task GetLeaderboard(CompetitionDTO dto, AuthSuccessResultDTO login)
        {
            HttpResponseMessage result = await Client.GetAsync($"/api/v1/competitions/{dto.ID}/participations");
            PlayerParticipationDTO[] l = await result.FromJSONAsync<PlayerParticipationDTO[]>();
            l.Should().ContainSingle().Which.AccountID.Should().Be(login.AccountID);
        }

        protected override async Task Init()
        {
            await Instantiate(new CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
        }
    }
}
