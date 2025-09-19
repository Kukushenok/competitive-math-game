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
namespace E2ETests
{
    public class CreateAccountThenParticipateE2ETest : E2ETest
    {
        public CreateAccountThenParticipateE2ETest(ITestOutputHelper helper, WebApplicationFactory<Program> p) : base(helper, p)
        {
            
        }
        [Fact]
        public async Task CompetitionFetchOne()
        {
            await ExecSQLFile("mix_no_rewards.sql");

            var acc = await CreateAccount();
            var comp = await GetCompetition();
            await Participate(comp);
            await GetLeaderboard(comp, acc);
        }
        [AllureStep("Create account")]
        private async Task<AuthSuccessResultDTO> CreateAccount()
        {
            
            var x = await Post<AccountCreationDTO, AuthSuccessResultDTO>("/api/v1/auth/register", new AccountCreationDTO(
                "kukushenok",
                "MySuperLongPassword",
                "mrcoolmoder@gmail.com"
            ));
            SetAuth(x.Token);
            return x;
        }
        [AllureStep("Get competition")]
        private async Task<CompetitionDTO> GetCompetition()
        {
            var result = await Client.GetAsync("/api/v1/competitions/?count=1");
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await GetObject<CompetitionDTO[]>(result);
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
            var L = await GetObject<PlayerParticipationDTO[]>(result);
            L.Should().ContainSingle().Which.AccountID.Should().Be(login.AccountID);
        }
    }
}
