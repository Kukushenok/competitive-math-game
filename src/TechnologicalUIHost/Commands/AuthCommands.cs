using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    public class AuthCommands : CompositeCommandBlock
    {
        private readonly IAuthCache authCache;
        private readonly IAuthUseCase useCase;
        public AuthCommands(IAuthCache cache, IAuthUseCase useCase)
            : base("Авторизация")
        {
            authCache = cache;
            this.useCase = useCase;
        }

        private async Task Register(IConsole console)
        {
            AccountCreationDTO dto = console.ReadAccountCreation();
            AuthSuccessResultDTO res = await useCase.Register(dto);
            authCache.Login(res);
        }

        private async Task Login(IConsole console)
        {
            AuthSuccessResultDTO res = await useCase.Login(console.ReadAccountLogin());
            authCache.Login(res);
        }

        private Task Logoff(IConsole console)
        {
            authCache.LogOff();
            return Task.CompletedTask;
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Регистрация", TaskDecorator.Sync(Register));
            yield return new CallbackConsoleMenuCommand("Вход в аккаунт", TaskDecorator.Sync(Login));
            yield return new CallbackConsoleMenuCommand("Выйти из аккаунта", TaskDecorator.Sync(Logoff), authCache.IsAuthed);
        }
    }

    public static class TaskDecorator
    {
        public static Action<IConsole> Sync(this Func<IConsole, Task> tsk)
        {
            return (IConsole c) => tsk(c).GetAwaiter().GetResult();
        }
    }
}
