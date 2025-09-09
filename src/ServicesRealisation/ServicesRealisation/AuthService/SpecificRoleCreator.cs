using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
namespace CompetitiveBackend.Services.AuthService
{
    public class BasicRoleCreator : IRoleCreator
    {
        public Role Create(Account data)
        {
            if (data.Login == "root") return new AdminRole();
            return new PlayerRole();
        }
    }
}
