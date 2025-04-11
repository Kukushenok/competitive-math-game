using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.BackendUsage.Objects;
using TechnologicalUI.Command;
using TechnologicalUI.MenuCommand;

namespace TechnologicalUI.Commands
{
    public class AuthCommands: CompositeCommandBlock
    {
        private IAuthCache authCache;
        private IAuthUseCase useCase;
        public AuthCommands(IAuthCache cache, IAuthUseCase useCase) : base("Авторизация")
        {
            authCache = cache;
            this.useCase = useCase;
        }
        private async Task Register()
        {
            AccountCreationDTO dto = new(CInput.ReadStr("Логин: "), CInput.ReadStr("Пароль: "), CInput.ReadStr("Электронная почта: "));
            var res = await useCase.Register(dto);
            authCache.Login(res);
        }
        private async Task Login()
        {
            var res = await useCase.Login(CInput.ReadStr("Логин: "), CInput.ReadStr("Пароль: "));
            authCache.Login(res);
        }
        private Task Logoff()
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
        public static Action Sync(this Func<Task> tsk)
        {
            return () => tsk().GetAwaiter().GetResult();
        }
    }
}
