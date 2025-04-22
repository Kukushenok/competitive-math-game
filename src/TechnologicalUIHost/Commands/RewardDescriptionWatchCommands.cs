using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    class RewardDescriptionWatchCommands: CompositeCommandBlock
    {
        private IRewardDescriptionWatchUseCase _watchUseCase;
        public RewardDescriptionWatchCommands(IRewardDescriptionWatchUseCase watchUseCase): base("Просмотр описаний наград")
        {
            _watchUseCase = watchUseCase;
        }
        private async Task GetAllRewards(IConsole console)
        {
            console.PrintEnumerable(await _watchUseCase.GetAllRewardDescriptions(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }
        private async Task GetRewardIcon(IConsole console)
        {
            console.Print(await _watchUseCase.GetRewardIcon(console.ReadInt("Введите ID описания награды: ")));
        }
        private async Task GetRewardGameAsset(IConsole console)
        {
            console.Print(await _watchUseCase.GetRewardGameAsset(console.ReadInt("Введите ID описания награды: ")));
        }
        private async Task GetRewardDescription(IConsole console)
        {
            console.Print(await _watchUseCase.GetRewardDescription(console.ReadInt("Введите ID описания награды: ")));
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить все описания наград", TaskDecorator.Sync(GetAllRewards));
            yield return new CallbackConsoleMenuCommand("Получить иконку награды", TaskDecorator.Sync(GetRewardIcon));
            yield return new CallbackConsoleMenuCommand("Получить игровое представление награды", TaskDecorator.Sync(GetRewardGameAsset));
            yield return new CallbackConsoleMenuCommand("Получить описание награды", TaskDecorator.Sync(GetRewardDescription));
        }
    }
}
