using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUI.Command;
using TechnologicalUI.MenuCommand;

namespace TechnologicalUI.Commands
{
    class PlayerRewardUseCaseCommands : AuthRequiringCommands
    {
        private IPlayerRewardUseCase _useCase;
        public PlayerRewardUseCaseCommands(IAuthCache cache, IPlayerRewardUseCase playerRewardUseCase) : base("Награды игрока", cache)
        {
            _useCase = playerRewardUseCase;
        }
        private async Task GrantRewardToPlayer(IConsole console)
        {
            using var self = await Auth(_useCase);
            self.GrantRewardToPlayer(console.ReadInt("Введите ID игрока > "), console.ReadInt("Введите ID типа награды > ")).GetAwaiter().GetResult();
        }

        private async Task DeleteReward(IConsole console)
        {
            using var self = await Auth(_useCase);
            self.DeleteReward(console.ReadInt("Введите ID награды игрока > ")).GetAwaiter().GetResult();
        }
        private async Task GetAllRewardsOf(IConsole console)
        {
            using var self = await Auth(_useCase);
            console.PrintEnumerable(await self.GetAllRewardsOf(console.ReadInt("Введите ID игрока > "), console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }
        private async Task GetAllMineRewards(IConsole console)
        {
            using var self = await Auth(_useCase);
            console.PrintEnumerable(await self.GetAllMineRewards(console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Наградить игрока", TaskDecorator.Sync(GrantRewardToPlayer), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Удалить награду", TaskDecorator.Sync(DeleteReward), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить награды игрока", TaskDecorator.Sync(GetAllRewardsOf), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Получить свои награды", TaskDecorator.Sync(GetAllMineRewards), IsAuthed);
        }
    }
}
