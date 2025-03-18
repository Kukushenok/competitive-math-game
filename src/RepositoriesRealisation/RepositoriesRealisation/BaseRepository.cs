using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    class BaseRepository
    {
        private IDbContextFactory<BaseDbContext> contextFactory;
        public BaseRepository(IDbContextFactory<BaseDbContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }
        protected async Task<BaseDbContext> GetDbContext() => await contextFactory.CreateDbContextAsync();
    }
}
