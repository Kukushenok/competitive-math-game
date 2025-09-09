using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    class RewardDescriptionEditCommands : AuthRequiringCommands
    {
        private IRewardDescriptionEditUseCase _editUseCase;
        public RewardDescriptionEditCommands(IAuthCache cache, IRewardDescriptionEditUseCase editUseCase) : base("Изменение описаний наград", cache)
        {
            _editUseCase = editUseCase;
        }
        private async Task CreateRewardDescription(IConsole console)
        {
            using var self = await Auth(_editUseCase);
            self.CreateRewardDescription(console.ReadRewardDescriptionDTO()).GetAwaiter().GetResult();
        }
        private async Task UpdateRewardDescription(IConsole console)
        {
            using var self = await Auth(_editUseCase);
            self.UpdateRewardDescription(console.ReadRewardDescriptionDTO()).GetAwaiter().GetResult();
        }
        private async Task SetRewardDescriptionIcon(IConsole console)
        {
            using var self = await Auth(_editUseCase);
            self.SetRewardIcon(console.ReadInt("Введите ID описания награды: "), console.ReadLargeDataDTO("Картинка")).GetAwaiter().GetResult();
        }
        private async Task SetRewardGameAsset(IConsole console)
        {
            using var self = await Auth(_editUseCase);
            self.SetRewardGameAsset(console.ReadInt("Введите ID описания награды: "), console.ReadLargeDataDTO("Награда")).GetAwaiter().GetResult();
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Создать описание награды", TaskDecorator.Sync(CreateRewardDescription), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить описание награды", TaskDecorator.Sync(UpdateRewardDescription), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Задать иконку награды", TaskDecorator.Sync(SetRewardDescriptionIcon), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Задать игровое представление награды", TaskDecorator.Sync(SetRewardGameAsset), IsAuthed);
        }
    }
}
