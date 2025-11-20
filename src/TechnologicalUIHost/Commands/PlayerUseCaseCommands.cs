using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    public class PlayerUseCaseCommands : CompositeCommandBlock
    {
        private readonly IPlayerProfileUseCase playerProfileUseCase;
        public PlayerUseCaseCommands(IPlayerProfileUseCase playerProfileUseCase)
            : base("Профили игроков")
        {
            this.playerProfileUseCase = playerProfileUseCase;
        }

        private async Task GetPlayerProfile(IConsole console)
        {
            PlayerProfileDTO p = await playerProfileUseCase.GetProfile(console.ReadInt("Введите ID профиля: "));
            console.Print(p);
        }

        private Task GetPlayerProfileImage(IConsole console)
        {
            LargeDataDTO p = playerProfileUseCase.GetProfileImage(console.ReadInt("Введите ID профиля: ")).GetAwaiter().GetResult();
            console.Print(p);
            return Task.CompletedTask;
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить профиль по ID", TaskDecorator.Sync(GetPlayerProfile));
            yield return new CallbackConsoleMenuCommand("Получить иконку профиля по ID", TaskDecorator.Sync(GetPlayerProfileImage));
        }
    }
}
