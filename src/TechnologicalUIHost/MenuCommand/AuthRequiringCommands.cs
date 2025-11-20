using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;

namespace TechnologicalUIHost.MenuCommand
{
    internal abstract class AuthRequiringCommands : CompositeCommandBlock
    {
        private readonly IAuthCache cache;

        protected AuthRequiringCommands(string name, IAuthCache cache)
            : base(name)
        {
            this.cache = cache;
        }

        protected bool IsAuthed()
        {
            return cache.IsAuthed();
        }

        protected string GetToken()
        {
            return cache.GetToken();
        }

        protected Task<T> Auth<T>(T something)
            where T : IAuthableUseCase<T>
        {
            return something.Auth(GetToken());
        }

        public override bool Enabled => IsAuthed();
    }
}
