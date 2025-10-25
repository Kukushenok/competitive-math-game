using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Repositories;
using RepositoriesRealisation.Models;

namespace RepositoriesRealisation
{
    public class BaseContextFactory : IDbContextFactory<BaseDbContext>
    {
        private readonly IConnectionStringGetter stringGetter;
        private readonly ILoggerFactory factory;
        public BaseContextFactory(IConnectionStringGetter configuration, ILoggerFactory factory)
        {
            stringGetter = configuration;
            this.factory = factory;
        }

        public BaseDbContext CreateDbContext()
        {
            string curPerms = stringGetter.GetConnectionString();
            var builder = new DbContextOptionsBuilder<BaseDbContext>();
            builder.UseNpgsql(curPerms, o => o.MapEnum<SupportedConditionType>("condition_type_enum"));
            builder.UseLoggerFactory(factory);
            var context = new BaseDbContext(builder.Options);
            return context;
        }
    }
}
