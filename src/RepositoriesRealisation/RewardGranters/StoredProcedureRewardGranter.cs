using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RewardGranters
{
    public class StoredProcedureRewardGranter : IRewardGranter
    {
        public async Task GrantRewards(BaseDbContext context, int competitionID)
        {
            // No possible injection: competitionID is integer.
            await context.Database.ExecuteSqlAsync($"call grant_rewards({competitionID})");
        }
    }
}
