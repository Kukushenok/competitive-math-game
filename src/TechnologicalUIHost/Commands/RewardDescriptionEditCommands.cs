using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class RewardDescriptionEditCommands : AuthRequiringCommands
    {
        private readonly IRewardDescriptionEditUseCase editUseCase;
        public RewardDescriptionEditCommands(IAuthCache cache, IRewardDescriptionEditUseCase editUseCase)
            : base("Изменение описаний наград", cache)
        {
            this.editUseCase = editUseCase;
        }

        private async Task CreateRewardDescription(IConsole console)
        {
            using IRewardDescriptionEditUseCase self = await Auth(editUseCase);
            self.CreateRewardDescription(console.ReadRewardDescriptionDTO()).GetAwaiter().GetResult();
        }

        private async Task UpdateRewardDescription(IConsole console)
        {
            using IRewardDescriptionEditUseCase self = await Auth(editUseCase);
            self.UpdateRewardDescription(console.ReadRewardDescriptionDTO()).GetAwaiter().GetResult();
        }

        private async Task SetRewardDescriptionIcon(IConsole console)
        {
            using IRewardDescriptionEditUseCase self = await Auth(editUseCase);
            self.SetRewardIcon(console.ReadInt("Введите ID описания награды: "), console.ReadLargeDataDTO("Картинка")).GetAwaiter().GetResult();
        }

        // private async Task SetRewardGameAsset(IConsole console)
        // {
        //    using var self = await Auth(_editUseCase);
        //    self.SetRewardGameAsset(console.ReadInt("Введите ID описания награды: "), console.ReadLargeDataDTO("Награда")).GetAwaiter().GetResult();
        // }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Создать описание награды", TaskDecorator.Sync(CreateRewardDescription), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить описание награды", TaskDecorator.Sync(UpdateRewardDescription), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Задать иконку награды", TaskDecorator.Sync(SetRewardDescriptionIcon), IsAuthed);

            // yield return new CallbackConsoleMenuCommand("Задать игровое представление награды", TaskDecorator.Sync(SetRewardGameAsset), IsAuthed);
        }
    }
}
