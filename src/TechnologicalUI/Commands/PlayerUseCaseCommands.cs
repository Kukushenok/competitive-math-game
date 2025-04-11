using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.Core.Objects;
using TechnologicalUI.Command;

namespace TechnologicalUI.Commands
{
    public class PlayerUseCaseCommands: CompositeCommandBlock
    {
        IPlayerProfileUseCase _playerProfileUseCase;
        public PlayerUseCaseCommands(IPlayerProfileUseCase playerProfileUseCase): base("Профили игроков")
        {
            _playerProfileUseCase = playerProfileUseCase;
        }
        private async Task GetPlayerProfile()
        {
            PlayerProfile p = await _playerProfileUseCase.GetProfile(CInput.ReadInt("Введите ID профиля: "));
            Console.WriteLine($"Описание: {p.Description}");
            Console.WriteLine($"Логин: {p.Name}");
            Console.WriteLine($"ID: {p.Id}");
        }
        private async Task GetPlayerProfileImage()
        {
            var p = _playerProfileUseCase.GetProfileImage(CInput.ReadInt("Введите ID профиля: ")).GetAwaiter().GetResult();
            string path = CInput.PromtInput("Сохранить в файл: ");
            await File.WriteAllBytesAsync(path, p.Data);
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Получить профиль по ID", TaskDecorator.Sync(GetPlayerProfile));
            yield return new CallbackConsoleMenuCommand("Получить иконку профиля по ID", TaskDecorator.Sync(GetPlayerProfileImage));
        }
    }
}
