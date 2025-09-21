using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using CompetitiveBackend.Controllers;
using Microsoft.AspNetCore.Mvc.Testing;
using CompetitiveBackend;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using Allure.Xunit.Attributes.Steps;
using Newtonsoft.Json;
using IntegrationalTests;
using System.Net.Http.Json;
using RepositoriesRealisation.Models;
using Bogus;
using Microsoft.EntityFrameworkCore;
namespace E2ETests
{
    public class CreateAccountThenParticipateE2ETest : IntegrationalTest
    {
        public CreateAccountThenParticipateE2ETest(IntegrationalFixture f) : base(f)
        {

        }

        [Fact]
        public async Task CompetitionFetchOne()
        {
            
            var acc = await CreateAccount();
            var comp = await GetCompetition();
            await Participate(comp);
            await GetLeaderboard(comp, acc);

            await RemoveAccount(acc);
        }
        [AllureStep("Create account")]
        private async Task<AuthSuccessResultDTO> CreateAccount()
        {
            
            var x = await Client.PostAsJsonAsync("/api/v1/auth/register", new AccountCreationDTO(
                Faker.Internet.UserName(),
                "MySuperLongPassword",
                "mrcoolmoder@gmail.com"
            ));
            var result = await x.FromJSONAsync<AuthSuccessResultDTO>();
            Client.DefaultRequestHeaders.Add("Bearer", result.Token);
            return result;
        }

        [AllureStep("Cleanup")]
        private async Task RemoveAccount(AuthSuccessResultDTO dto)
        {
            int idx = dto.AccountID;
            await Context.PlayerParticipation.Where(x => x.AccountID == idx).ExecuteDeleteAsync();
            var accs = await Context.AccountsReadOnly.Where(x => x.Id == idx).ToListAsync();
            Context.AccountsReadOnly.RemoveRange(accs);
            await Context.SaveChangesAsync();
        }

        [AllureStep("Get competition")]
        private async Task<CompetitionDTO> GetCompetition()
        {
            var result = await Client.GetAsync("/api/v1/competitions/?count=1");
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Should().ContainSingle();
            return obj[0];
        }
        [AllureStep("Participate")]
        private async Task Participate(CompetitionDTO dto)
        {
            var x = await Client.PutAsync($"/api/v1/competitions/{dto.ID}/participations/?score=500", null);
            x.IsSuccessStatusCode.Should().BeTrue();
        }
        [AllureStep("Get leaderboard")]
        private async Task GetLeaderboard(CompetitionDTO dto, AuthSuccessResultDTO login)
        {
            var result = await Client.GetAsync($"/api/v1/competitions/{dto.ID}/participations");
            var L = await result.FromJSONAsync<PlayerParticipationDTO[]>();
            L.Should().ContainSingle().Which.AccountID.Should().Be(login.AccountID);
        }
        protected override async Task Init()
        {
            await Instantiate(new CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
        }
    }
}
