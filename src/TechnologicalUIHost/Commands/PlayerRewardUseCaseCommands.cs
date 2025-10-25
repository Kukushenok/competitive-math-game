using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class PlayerRewardUseCaseCommands : AuthRequiringCommands
    {
        private readonly IPlayerRewardUseCase useCase;
        public PlayerRewardUseCaseCommands(IAuthCache cache, IPlayerRewardUseCase playerRewardUseCase)
            : base("Награды игрока", cache)
        {
            useCase = playerRewardUseCase;
        }

        private async Task GrantRewardToPlayer(IConsole console)
        {
            using IPlayerRewardUseCase self = await Auth(useCase);
            self.GrantRewardToPlayer(console.ReadInt("Введите ID игрока > "), console.ReadInt("Введите ID типа награды > ")).GetAwaiter().GetResult();
        }

        private async Task DeleteReward(IConsole console)
        {
            using IPlayerRewardUseCase self = await Auth(useCase);
            self.DeleteReward(console.ReadInt("Введите ID награды игрока > ")).GetAwaiter().GetResult();
        }

        private async Task GetAllRewardsOf(IConsole console)
        {
            using IPlayerRewardUseCase self = await Auth(useCase);
            console.PrintEnumerable(await self.GetAllRewardsOf(console.ReadInt("Введите ID игрока > "), console.ReadDataLimiterDTO()), (c, x) => c.Print(x));
        }

        private async Task GetAllMineRewards(IConsole console)
        {
            using IPlayerRewardUseCase self = await Auth(useCase);
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
