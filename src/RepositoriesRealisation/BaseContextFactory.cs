using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RepositoriesRealisation
{
    public class BaseContextFactory : IDbContextFactory<BaseDbContext>
    {
        private IConfiguration _configuration;
        public BaseContextFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public BaseDbContext CreateDbContext()
        {
            string curPerms = _configuration.GetConnectionString("postgres")!;
            var builder = new DbContextOptionsBuilder<BaseDbContext>();
            builder.UseNpgsql(curPerms);
            BaseDbContext context = new BaseDbContext(builder.Options);
            return context;
        }
    }
}
