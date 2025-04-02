using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Repositories;
using RepositoriesRealisation.RewardGranters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation
{
    public class Options(IServiceCollection coll)
    {
        public bool SetUpRewardGranter { get; private set; } = false;
        public bool SetUpConnectionStringGetter { get; private set; } = false;
        public Options GrantRewardsWithStoredProcedure(string procedureName = "grant_rewards")
        {
            SetUpRewardGranter = true;
            coll.AddScoped<IRewardGranter, StoredProcedureRewardGranter>((IServiceProvider vr) => new StoredProcedureRewardGranter(procedureName));
            return this;
        }
        public Options GrantRewardsWithDefaultCalls()
        {
            SetUpRewardGranter = true;
            coll.AddScoped<IRewardGranter, BasicRewardGranter>();
            return this;
        }
        public Options UseDefaultConnectionString(string connectionString = "postgres")
        {
            SetUpConnectionStringGetter = true;
            coll.AddScoped<IConnectionStringGetter>((IServiceProvider p) => new ConfigurationConnectionStringGetter(p.GetService<IConfiguration>()!, connectionString));
            return this;
        }
        public Options UseCustomConnectionStringGetter(IConnectionStringGetter getter)
        {
            SetUpConnectionStringGetter = true;
            coll.AddSingleton(getter);
            return this;
        }
    }
}
