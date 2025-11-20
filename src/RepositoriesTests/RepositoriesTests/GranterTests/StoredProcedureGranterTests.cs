using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public class StoredProcedureGranterTests : BaseGranterTests
    {
        public StoredProcedureGranterTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        protected override string Name => "stored";

        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(options => options.GrantRewardsWithStoredProcedure("grant_rewards"));
        }
    }
}
