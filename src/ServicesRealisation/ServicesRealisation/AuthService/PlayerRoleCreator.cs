using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Services.Objects;
namespace CompetitiveBackend.Services.AuthService
{
    public class PlayerRoleCreator : IRoleCreator { public Role Create(Account data) => new PlayerRole(); }
}
