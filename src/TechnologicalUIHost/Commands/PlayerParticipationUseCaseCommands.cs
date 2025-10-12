using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    class PlayerParticipationWatchUseCaseCommands: CompositeCommandBlock
    {
        private IPlayerParticipationWatchUseCase _useCase;
        public PlayerParticipationWatchUseCaseCommands(IPlayerParticipationWatchUseCase useCase) : base("Просмотр заявок")
        {
            _useCase = useCase;
        }
        public async Task GetParticipation(IConsole console)
        {
            var result = await _useCase.GetParticipation(
                console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите ID игрока: "));
            console.Print(result);
        }
        public async Task GetLeaderboard(IConsole console)
        {
            console.PrintEnumerable(await _useCase.GetLeaderboard(console.ReadInt("Введите ID соревнования: "), console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить список лидеров", TaskDecorator.Sync(GetLeaderboard));
            yield return new CallbackConsoleMenuCommand("Получить информацию о заявке", TaskDecorator.Sync(GetParticipation));
        }
    }
    class PlayerParticipationUseCaseCommands: AuthRequiringCommands 
    {
        private IPlayerParticipationUseCase _playerParticipationUseCase;
        public PlayerParticipationUseCaseCommands(IAuthCache cache, IPlayerParticipationUseCase playerParticipationUseCase): base("Просмотр участников", cache)
        {
            this._playerParticipationUseCase = playerParticipationUseCase;
        }

        //private async Task SubmitScoreTo(IConsole console)
        //{
        //    using var self = await Auth(_playerParticipationUseCase);
        //    self.SubmitScoreTo(console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите свой результат: ")).GetAwaiter().GetResult();
        //}
        private async Task DeleteParticipation(IConsole console)
        {
            using var self = await Auth(_playerParticipationUseCase);
            self.DeleteParticipation(console.ReadInt("Введите ID соревнования: "), console.ReadInt("Введите ID игрока: ")).GetAwaiter().GetResult();
        }
        private async Task GetMyParticipations(IConsole console)
        {
            using var self = await Auth(_playerParticipationUseCase);
            console.PrintEnumerable(await self.GetMyParticipations(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            //yield return new CallbackConsoleMenuCommand("Подать заявку на соревнование", TaskDecorator.Sync(SubmitScoreTo), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Удалить заявку на соревнование", TaskDecorator.Sync(DeleteParticipation), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить свои заявки на соревнования", TaskDecorator.Sync(GetMyParticipations), IsAuthed);
        }
    }
}
