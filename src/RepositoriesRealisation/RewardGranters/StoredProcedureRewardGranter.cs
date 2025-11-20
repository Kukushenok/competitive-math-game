using System.Runtime.CompilerServices;
using CompetitiveBackend.Repositories.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace RepositoriesRealisation.RewardGranters
{
    public class StoredProcedureRewardGranter : IRewardGranter
    {
        private readonly string procedureName;
        public StoredProcedureRewardGranter(string name)
        {
            procedureName = name;
        }

        public async Task GrantRewards(BaseDbContext context, int competitionID)
        {
            // No possible injection: competitionID is integer.
            try
            {
                await context.Database.ExecuteSqlAsync(FormattableStringFactory.Create($"call {procedureName}({{0}})".ToString(), competitionID));
            }
            catch (Npgsql.PostgresException postgresException)
            {
                throw new FailedOperationException(postgresException.Message + "\n" + postgresException.Hint);
            }
        }
    }
}
