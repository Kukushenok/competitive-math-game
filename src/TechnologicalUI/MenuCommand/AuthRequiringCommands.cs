using CompetitiveBackend.BackendUsage;
using TechnologicalUI.Command;

namespace TechnologicalUI.MenuCommand
{
    abstract class AuthRequiringCommands : CompositeCommandBlock
    {
        IAuthCache _cache;

        protected AuthRequiringCommands(string name, IAuthCache cache) : base(name)
        {
            this._cache = cache;
        }
        protected bool IsAuthed() => _cache.IsAuthed();
        protected string GetToken() => _cache.GetToken();
        protected Task<T> Auth<T>(T something) where T : IAuthableUseCase<T> 
        {
            return something.Auth(GetToken());
        }

    }
    
}
