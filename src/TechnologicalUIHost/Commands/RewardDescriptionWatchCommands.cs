using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    internal sealed class RewardDescriptionWatchCommands : CompositeCommandBlock
    {
        private readonly IRewardDescriptionWatchUseCase watchUseCase;
        public RewardDescriptionWatchCommands(IRewardDescriptionWatchUseCase watchUseCase)
            : base("Просмотр описаний наград")
        {
            this.watchUseCase = watchUseCase;
        }

        private async Task GetAllRewards(IConsole console)
        {
            console.PrintEnumerable(await watchUseCase.GetAllRewardDescriptions(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }

        private async Task GetRewardIcon(IConsole console)
        {
            console.Print(await watchUseCase.GetRewardIcon(console.ReadInt("Введите ID описания награды: ")));
        }

        // private async Task GetRewardGameAsset(IConsole console)
        // {
        //    console.Print(await _watchUseCase.GetRewardGameAsset(console.ReadInt("Введите ID описания награды: ")));
        // }
        private async Task GetRewardDescription(IConsole console)
        {
            console.Print(await watchUseCase.GetRewardDescription(console.ReadInt("Введите ID описания награды: ")));
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить все описания наград", TaskDecorator.Sync(GetAllRewards));
            yield return new CallbackConsoleMenuCommand("Получить иконку награды", TaskDecorator.Sync(GetRewardIcon));

            // yield return new CallbackConsoleMenuCommand("Получить игровое представление награды", TaskDecorator.Sync(GetRewardGameAsset));
            yield return new CallbackConsoleMenuCommand("Получить описание награды", TaskDecorator.Sync(GetRewardDescription));
        }
    }
}
