using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesRealisation.ServicesRealisation.Validator.Constraints
{
    public interface IConfigReadable
    {
        public void Read(IConfigurationSection section);
    }
}
