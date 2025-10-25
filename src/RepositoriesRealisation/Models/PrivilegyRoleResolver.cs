using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories.Exceptions;

namespace RepositoriesRealisation.DatabaseObjects
{
    public class PrivilegyRoleResolver
    {
        public static int Resolve(Role rl)
        {
            return rl.IsPlayer() ? 0 : rl.IsAdmin() ? 1 : throw new FailedOperationException("No such role supported");
        }

        public static Role Resolve(int rl)
        {
            return rl == 0 ? new PlayerRole() : rl == 1 ? (Role)new AdminRole() : throw new FailedOperationException("No such role supported");
        }
    }
}
