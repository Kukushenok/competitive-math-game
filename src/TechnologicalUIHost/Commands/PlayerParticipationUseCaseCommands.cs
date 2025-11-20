using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class PlayerParticipationWatchUseCaseCommands : CompositeCommandBlock
    {
        private readonly IPlayerParticipationWatchUseCase useCase;
        public PlayerParticipationWatchUseCaseCommands(IPlayerParticipationWatchUseCase useCase)
            : base("Просмотр заявок")
        {
            this.useCase = useCase;
        }

        public async Task GetParticipation(IConsole console)
        {
            CompetitiveBackend.BackendUsage.Objects.PlayerParticipationDTO result = await useCase.GetParticipation(
                console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите ID игрока: "));
            console.Print(result);
        }

        public async Task GetLeaderboard(IConsole console)
        {
            console.PrintEnumerable(await useCase.GetLeaderboard(console.ReadInt("Введите ID соревнования: "), console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить список лидеров", TaskDecorator.Sync(GetLeaderboard));
            yield return new CallbackConsoleMenuCommand("Получить информацию о заявке", TaskDecorator.Sync(GetParticipation));
        }
    }

    internal sealed class PlayerParticipationUseCaseCommands : AuthRequiringCommands
    {
        private readonly IPlayerParticipationUseCase playerParticipationUseCase;
        public PlayerParticipationUseCaseCommands(IAuthCache cache, IPlayerParticipationUseCase playerParticipationUseCase)
            : base("Просмотр участников", cache)
        {
            this.playerParticipationUseCase = playerParticipationUseCase;
        }

        // private async Task SubmitScoreTo(IConsole console)
        // {
        //    using var self = await Auth(_playerParticipationUseCase);
        //    self.SubmitScoreTo(console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите свой результат: ")).GetAwaiter().GetResult();
        // }
        private async Task DeleteParticipation(IConsole console)
        {
            using IPlayerParticipationUseCase self = await Auth(playerParticipationUseCase);
            self.DeleteParticipation(console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите ID игрока: ")).GetAwaiter().GetResult();
        }

        private async Task GetMyParticipations(IConsole console)
        {
            using IPlayerParticipationUseCase self = await Auth(playerParticipationUseCase);
            console.PrintEnumerable(await self.GetMyParticipations(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            // yield return new CallbackConsoleMenuCommand("Подать заявку на соревнование", TaskDecorator.Sync(SubmitScoreTo), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Удалить заявку на соревнование", TaskDecorator.Sync(DeleteParticipation), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить свои заявки на соревнования", TaskDecorator.Sync(GetMyParticipations), IsAuthed);
        }
    }
}
