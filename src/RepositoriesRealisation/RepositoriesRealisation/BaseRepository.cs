using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal class BaseRepository<T>
    {
        private readonly IDbContextFactory<BaseDbContext> contextFactory;
        protected ILogger<T> logger;
        public BaseRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<T> logger)
        {
            this.contextFactory = contextFactory;
            this.logger = logger;
        }

        protected async Task<BaseDbContext> GetDbContext()
        {
            return await contextFactory.CreateDbContextAsync();
        }
    }
}
