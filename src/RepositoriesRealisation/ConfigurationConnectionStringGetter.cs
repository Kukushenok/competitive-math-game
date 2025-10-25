using Microsoft.Extensions.Configuration;
using Repositories.Repositories;

namespace RepositoriesRealisation
{
    internal sealed class ConfigurationConnectionStringGetter : IConnectionStringGetter
    {
        private readonly IConfiguration conf;
        private readonly string connStringName;
        public ConfigurationConnectionStringGetter(IConfiguration conf, string connStringName)
        {
            this.conf = conf;
            this.connStringName = connStringName;
        }

        public string GetConnectionString()
        {
            return conf.GetConnectionString(connStringName)!;
        }
    }
}
