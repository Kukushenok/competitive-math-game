using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class CompetitionEditCommands : AuthRequiringCommands
    {
        private readonly ICompetitionEditUseCase editUseCase;
        public CompetitionEditCommands(IAuthCache cache, ICompetitionEditUseCase editUseCase)
            : base("Изменить соревнования", cache)
        {
            this.editUseCase = editUseCase;
        }

        private async Task CreateCompetition(IConsole console)
        {
            using ICompetitionEditUseCase self = await Auth(editUseCase);
            await self.CreateCompetition(console.ReadCompetitionDTO());
        }

        private async Task UpdateCompetition(IConsole console)
        {
            using ICompetitionEditUseCase self = await Auth(editUseCase);
            await self.UpdateCompetition(console.ReadCompetitionUpdateRequestDTO());
        }

        // private async Task SetCompetitionLevel(IConsole console)
        // {
        //    using var self = await Auth(editUseCase);
        //    await self.SetCompetitionLevel(console.ReadInt("ID соревнования > "), console.ReadLargeDataDTO("Уровень > "));
        // }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Создать соревнование", TaskDecorator.Sync(CreateCompetition), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить соревнование", TaskDecorator.Sync(UpdateCompetition), IsAuthed);

            // yield return new CallbackConsoleMenuCommand("Задать уровень соревнования", TaskDecorator.Sync(SetCompetitionLevel), IsAuthed);
        }
    }
}
