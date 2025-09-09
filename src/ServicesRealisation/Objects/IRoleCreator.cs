using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services.Objects
{
    public interface IRoleCreator
    {
        public Role Create(Account c);
    }
}
