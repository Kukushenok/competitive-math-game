using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.Objects
{
    public record AuthSuccessResultDTO(string Token, string RoleName, int AccountID);
}
