using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
namespace CompetitiveBackend.Services.AuthService
{
    public class BasicRoleCreator : IRoleCreator
    {
        public Role Create(Account c)
        {
            return c.Login == "root" ? new AdminRole() : new PlayerRole();
        }
    }
}
