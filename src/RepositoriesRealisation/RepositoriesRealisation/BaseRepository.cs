using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
