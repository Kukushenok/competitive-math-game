using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RewardGranters
{
    public class StoredProcedureRewardGranter : IRewardGranter
    {
        string _procedureName;
        public StoredProcedureRewardGranter(string name)
        {
            _procedureName = name;
        }
        public async Task GrantRewards(BaseDbContext context, int competitionID)
        {
            // No possible injection: competitionID is integer.
            try
            {
                await context.Database.ExecuteSqlAsync(FormattableStringFactory.Create($"call {_procedureName}({{0}})".ToString(), competitionID));
            }
            catch(Npgsql.PostgresException postgresException)
            {
                throw new FailedOperationException(postgresException.Message + "\n"+postgresException.Hint);
            }
        }
    }
}
