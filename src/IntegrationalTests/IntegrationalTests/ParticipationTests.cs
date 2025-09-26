using CompetitiveBackend;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Controllers;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Repositories;
using CompetitiveBackend.Repositories.Exceptions;
using FluentAssertions;
using IntegrationalTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
namespace IntegrationalTests
{
    public class ParticipationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private RepositoriesRealisation.Models.CompetitionModel tracking;

        [Fact]
        public async Task Participate()
        {
            await using var REGISTERACC = new SetupAuth(Client, Context, Faker.Internet.UserName());
            var ACCOUNT = await REGISTERACC.RegisterAndLogIn();
            
            // Act
            var result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null);//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            await Context.PlayerParticipation.Where(x => x.AccountID == ACCOUNT.AccountID).ExecuteDeleteAsync();
        }
        [Fact]
        public async Task ParticipateNoLoggedIn()
        {
            // Act
            var result = await Client.PutAsync($"/api/v1/competitions/{tracking.Id}/participations/?score=500", null);//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            var x = Context.PlayerParticipation.Where(x => x.CompetitionID == tracking.Id).ToList();
            x.Should().BeEmpty();
        }
        protected override async Task Init()
        {
            tracking = await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            
        }
    }
}
