using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoriesRealisation;

namespace CompetitiveBackend.Repositories
{
    internal class BaseRepository<T>
    {
        private IDbContextFactory<BaseDbContext> contextFactory;
        protected ILogger<T> _logger;
        public BaseRepository(IDbContextFactory<BaseDbContext> contextFactory, ILogger<T> logger)
        {
            this.contextFactory = contextFactory;
            _logger = logger;
        }
        protected async Task<BaseDbContext> GetDbContext() => await contextFactory.CreateDbContextAsync();
    }
}
