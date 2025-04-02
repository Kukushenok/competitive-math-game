using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Repositories.Repositories;

namespace RepositoriesRealisation
{
    public class BaseContextFactory : IDbContextFactory<BaseDbContext>
    {
        private IConnectionStringGetter _stringGetter;
        private ILoggerFactory _factory;
        public BaseContextFactory(IConnectionStringGetter configuration, ILoggerFactory factory)
        {
            _stringGetter = configuration;
            _factory = factory;
        }
        public BaseDbContext CreateDbContext()
        {
            string curPerms = _stringGetter.GetConnectionString();
            var builder = new DbContextOptionsBuilder<BaseDbContext>();
            builder.UseNpgsql(curPerms);
            builder.UseLoggerFactory(_factory);
            BaseDbContext context = new BaseDbContext(builder.Options);
            return context;
        }
    }
}
