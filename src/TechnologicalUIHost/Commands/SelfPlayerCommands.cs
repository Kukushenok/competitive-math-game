using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class SelfPlayerCommands : AuthRequiringCommands
    {
        private readonly ISelfUseCase selfUseCase;
        public SelfPlayerCommands(ISelfUseCase useCase, IAuthCache cache)
            : base("О себе", cache)
        {
            selfUseCase = useCase;
        }

        private async Task GetMyProfile(IConsole console)
        {
            using ISelfUseCase self = await Auth(selfUseCase);
            PlayerProfileDTO p = self.GetMyProfile().GetAwaiter().GetResult();
            console.Print(p);
        }

        private async Task GetMyImage(IConsole console)
        {
            using ISelfUseCase self = await Auth(selfUseCase);
            LargeDataDTO p = self.GetMyImage().GetAwaiter().GetResult();
            console.Print(p);
        }

        private async Task UpdateMyProfile(IConsole console)
        {
            using ISelfUseCase self = await Auth(selfUseCase);
            PlayerProfileDTO p = console.ReadPlayerProfileDTO();
            await self.UpdateMyPlayerProfile(p);
        }

        private async Task UpdateMyImage(IConsole console)
        {
            using ISelfUseCase self = await Auth(selfUseCase);
            await self.UpdateMyImage(console.ReadLargeDataDTO());
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Мой профиль", TaskDecorator.Sync(GetMyProfile), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Моя картинка", TaskDecorator.Sync(GetMyImage), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить мой профиль", TaskDecorator.Sync(UpdateMyProfile), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить мою картинку", TaskDecorator.Sync(UpdateMyImage), IsAuthed);
        }
    }
}
