using Microsoft.Extensions.DependencyInjection;
using RepositoriesRealisation;
using Xunit.Abstractions;

namespace RepositoriesTests.RepositoriesTests.GranterTests
{
    public class BasicRewardGranterTests : BaseGranterTests
    {
        public BasicRewardGranterTests(ITestOutputHelper helper)
            : base(helper)
        {
        }

        protected override string Name => "basic";

        protected override void AddMyRepositories(IServiceCollection coll)
        {
            coll.AddCompetitiveRepositories(options => options.GrantRewardsWithDefaultCalls());
        }
    }
}
