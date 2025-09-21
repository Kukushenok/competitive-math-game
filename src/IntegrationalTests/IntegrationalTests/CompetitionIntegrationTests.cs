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
using IntegrationalTests;
namespace IntegrationalTests
{
    public class CompetitionIntegrationTests(IntegrationalFixture f) : IntegrationalTest(f)
    {
        private RepositoriesRealisation.Models.CompetitionModel tracking;

        [Fact]
        public async Task CompetitionFetchOne()
        {
            // Act
            var result = await Client.GetAsync($"/api/v1/competitions/{tracking.Id}");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await result.FromJSONAsync<CompetitionDTO>();
            obj.ID.Should().Be(tracking.Id);
            obj.Name.Should().Be(tracking.Name);
        }
        [Fact]
        public async Task CompetitionFetchAll()
        {
            // Act
            var result = await Client.GetAsync("/api/v1/competitions/");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Count().Should().BeGreaterThanOrEqualTo(2);
        }
        [Fact]
        public async Task CompetitionFetchPage()
        {
            // Act
            var result = await Client.GetAsync("/api/v1/competitions/?page=1&count=1");//await Client.GetAsync($"{APIConsts.ROOTV1}/{APIConsts.COMPETITIONS}/1");

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            var obj = await result.FromJSONAsync<CompetitionDTO[]>();
            obj.Should().ContainSingle();
        }

        protected override async Task Init()
        {
            tracking = await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("A", "B", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            await Instantiate(new RepositoriesRealisation.Models.CompetitionModel("C", "D", DateTime.UtcNow, DateTime.UtcNow.AddDays(2)));
            
        }
    }
}
