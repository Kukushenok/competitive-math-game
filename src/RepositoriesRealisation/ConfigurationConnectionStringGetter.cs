using Microsoft.Extensions.Configuration;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation
{
    internal class ConfigurationConnectionStringGetter : IConnectionStringGetter
    {
        private IConfiguration conf;
        private string connStringName;
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
