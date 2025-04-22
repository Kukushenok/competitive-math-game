using CompetitiveBackend.Core.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Objects
{
    public interface IRepositoryPrivilegySetting
    {
        public void SetPrivilegies(string privilegy);
        public void SetPrivilegies(SessionToken token) => SetPrivilegies(token.Role.ToString());
    }
}
