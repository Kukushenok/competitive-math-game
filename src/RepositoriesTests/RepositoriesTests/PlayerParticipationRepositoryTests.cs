using CompetitiveBackend.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests
{
    public class PlayerParticipationRepositoryTests : IntegrationTest<IPlayerParticipationRepository>
    {
        public PlayerParticipationRepositoryTests(ITestOutputHelper helper) : base(helper)
        {
        }
    }
}
