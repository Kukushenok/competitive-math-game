using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.Core.Objects;
using TechnologicalUI.Command;
using TechnologicalUI.MenuCommand;

namespace TechnologicalUI.Commands
{
    class SelfPlayerCommands : AuthRequiringCommands
    {
        ISelfUseCase selfUseCase;
        public SelfPlayerCommands(ISelfUseCase useCase, IAuthCache cache) : base("О себе", cache)
        {
            selfUseCase = useCase;
        }
        private async Task GetMyProfile()
        {
            using(var self = await Auth(selfUseCase))
            {
                var p = self.GetMyProfile().GetAwaiter().GetResult();
                Console.WriteLine($"Описание: {p.Description}");
                Console.WriteLine($"Логин: {p.Name}");
                Console.WriteLine($"ID: {p.Id}");
            }
        }
        private async Task GetMyImage()
        {
            using (var self = await Auth(selfUseCase))
            {
                var p = self.GetMyImage().GetAwaiter().GetResult();
                string path = CInput.PromtInput("Сохранить в файл: ");
                await File.WriteAllBytesAsync(path, p.Data);
            }
        }
        private async Task UpdateMyProfile()
        {
            using (var self = await Auth(selfUseCase))
            {
                PlayerProfile p = new PlayerProfile(CInput.ReadStr("Логин: "), CInput.ReadStr("Описание: "));
                await self.UpdateMyPlayerProfile(p);
            }
        }
        private async Task UpdateMyImage()
        {
            using(var self = await Auth(selfUseCase))
            {
                await self.UpdateMyImage(CInput.ReadLD("Изображение профиля: "));
            }
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Мой профиль", TaskDecorator.Sync(GetMyProfile), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Моя картинка", TaskDecorator.Sync(GetMyImage), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить мой профиль", TaskDecorator.Sync(UpdateMyProfile), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить мою картинку", TaskDecorator.Sync(UpdateMyImage), IsAuthed);
        }
        public override bool Enabled { get => IsAuthed(); }
    }
}
