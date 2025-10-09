using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Commands
{
    internal class CompetitionEditCommands : AuthRequiringCommands
    {
        private ICompetitionEditUseCase editUseCase;
        public CompetitionEditCommands(IAuthCache cache, ICompetitionEditUseCase editUseCase) : base("Изменить соревнования", cache)
        {
            this.editUseCase = editUseCase;
        }
        private async Task CreateCompetition(IConsole console)
        {
            using var self = await Auth(editUseCase);
            await self.CreateCompetition(console.ReadCompetitionDTO());
        }
        private async Task UpdateCompetition(IConsole console)
        {
            using var self = await Auth(editUseCase);
            await self.UpdateCompetition(console.ReadCompetitionUpdateRequestDTO());
        }
        //private async Task SetCompetitionLevel(IConsole console)
        //{
        //    using var self = await Auth(editUseCase);
        //    await self.SetCompetitionLevel(console.ReadInt("ID соревнования > "), console.ReadLargeDataDTO("Уровень > "));
        //}
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Создать соревнование", TaskDecorator.Sync(CreateCompetition), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновить соревнование", TaskDecorator.Sync(UpdateCompetition), IsAuthed);
           // yield return new CallbackConsoleMenuCommand("Задать уровень соревнования", TaskDecorator.Sync(SetCompetitionLevel), IsAuthed);

        }
    }
}
