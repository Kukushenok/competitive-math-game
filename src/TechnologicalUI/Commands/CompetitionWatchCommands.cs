using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUI.Command;

namespace TechnologicalUI.Commands
{
    internal class CompetitionWatchCommands: CompositeCommandBlock
    {
        private ICompetitionWatchUseCase watchUseCase;
        public CompetitionWatchCommands(ICompetitionWatchUseCase watchUseCase): base("Просмотр соревнований")
        {
            this.watchUseCase = watchUseCase;
        }
        private async Task GetAllCompetitions(IConsole console)
        {
            console.PrintEnumerable(await watchUseCase.GetAllCompetitions(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }
        private async Task GetAllActiveCompetitions(IConsole console)
        {
            console.PrintEnumerable(await watchUseCase.GetActiveCompetitions(), (c, x) => c.Print(x));
        }
        private async Task GetCompetition(IConsole console)
        {
            console.Print(await watchUseCase.GetCompetition(console.ReadInt("Введите ID соревнования: ")));
        }
        private async Task GetCompetitionLevel(IConsole console)
        {
            console.Print(await watchUseCase.GetCompetitionLevel(console.ReadInt("Введите ID соревнования: ")));
        }
        private async Task GetRewardsFor(IConsole console)
        {
            console.PrintEnumerable(await watchUseCase.GetRewardsFor(console.ReadInt("Введите ID соревнования: ")), (c, x) => c.Print(x));
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Список всех соревнований", TaskDecorator.Sync(GetAllCompetitions));
            yield return new CallbackConsoleMenuCommand("Список активных соревнований", TaskDecorator.Sync(GetAllActiveCompetitions));
            yield return new CallbackConsoleMenuCommand("Информация о соревновании", TaskDecorator.Sync(GetCompetition));
            yield return new CallbackConsoleMenuCommand("Получить уровень соревнования", TaskDecorator.Sync(GetCompetitionLevel));
            yield return new CallbackConsoleMenuCommand("Получить награды за соревнование", TaskDecorator.Sync(GetRewardsFor));
        }
    }
}
