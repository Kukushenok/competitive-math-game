using CompetitiveBackend.Core.Auth;

namespace Repositories.Objects
{
    public interface IRepositoryPrivilegySetting
    {
        void SetPrivilegies(string privilegy);
        void SetPrivilegies(SessionToken token)
        {
            SetPrivilegies(token.Role.ToString());
        }
    }
}
