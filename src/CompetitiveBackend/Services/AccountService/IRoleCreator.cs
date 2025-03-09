using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.LogicComponents;
namespace CompetitiveBackend.Services.AccountSevice
{
    public interface IRoleCreator
    {
        public Role Create(Account data);
    }
}
