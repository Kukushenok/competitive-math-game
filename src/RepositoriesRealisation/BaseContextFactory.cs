using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace RepositoriesRealisation
{
    public class BaseContextFactory : IDbContextFactory<BaseDbContext>
    {
        private IConfiguration _configuration;
        private ILoggerFactory _factory;
        public BaseContextFactory(IConfiguration configuration, ILoggerFactory factory)
        {
            _configuration = configuration;
            _factory = factory;
        }
        public BaseDbContext CreateDbContext()
        {
            string curPerms = _configuration.GetConnectionString("postgres")!;
            var builder = new DbContextOptionsBuilder<BaseDbContext>();
            builder.UseNpgsql(curPerms);
            builder.UseLoggerFactory(_factory);
            BaseDbContext context = new BaseDbContext(builder.Options);
            return context;
        }
    }
}
