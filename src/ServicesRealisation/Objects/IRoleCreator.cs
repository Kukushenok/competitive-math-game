using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.Services.Objects
{
    public interface IRoleCreator
    {
        Role Create(Account c);
    }
}
