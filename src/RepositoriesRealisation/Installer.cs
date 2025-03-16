using CompetitiveBackend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation
{
    public static class Installer
    {
        public static IServiceCollection AddRepositories(this IServiceCollection container)
        {
            container.AddScoped<IAccountRepository, AccountRepository>();
            container.AddScoped<ISessionRepository, SessionRepository>();
            container.AddScoped<SessionRepositoryConfiguration>();
            container.AddDbContextFactory<BaseDbContext, BaseContextFactory>();
            return container;
        }
    }
}
