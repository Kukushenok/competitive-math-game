using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Repositories.Exceptions;

namespace RepositoriesRealisation.DatabaseObjects
{
    public class PrivilegyRoleResolver
    {
        public static int Resolve(Role rl)
        {
            if (rl.IsPlayer()) return 0;
            if (rl.IsAdmin()) return 1;
            throw new FailedOperationException("No such role supported");
        }
        public static Role Resolve(int rl)
        {
            if (rl == 0) return new PlayerRole();
            if (rl == 1) return new AdminRole();
            throw new FailedOperationException("No such role supported");
        }
    }
}
