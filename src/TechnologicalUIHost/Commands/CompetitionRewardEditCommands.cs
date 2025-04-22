using CompetitiveBackend.BackendUsage.Objects;
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
    class CompetitionRewardEditCommands: AuthRequiringCommands
    {
        private ICompetitionRewardEditUseCase _rewardEditUseCase;

        public CompetitionRewardEditCommands(IAuthCache cache, ICompetitionRewardEditUseCase useCase) : base("Изменение наград за соревнования", cache)
        {
            _rewardEditUseCase = useCase;
        }
        private async Task CreateCompetitionReward(IConsole console)
        {
            using var self = await Auth(_rewardEditUseCase);
            self.CreateCompetitionReward(console.ReadCreateCompetitionRewardDO()).GetAwaiter().GetResult();
        }
        private async Task UpdateCompetitionReward(IConsole console)
        {
            using var self = await Auth(_rewardEditUseCase);
            self.UpdateCompetitionReward(console.ReadUpdateCompetitionRewardDTO()).GetAwaiter().GetResult();
        }
        private async Task RemoveCompetitionReward(IConsole console)
        {
            using var self = await Auth(_rewardEditUseCase);
            self.RemoveCompetitionReward(console.ReadInt("Введите ID награды, которую нужно удалить: ")).GetAwaiter().GetResult();
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return new CallbackConsoleMenuCommand("Создание награды", TaskDecorator.Sync(CreateCompetitionReward), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Обновление награды", TaskDecorator.Sync(UpdateCompetitionReward), IsAuthed);
            yield return new CallbackConsoleMenuCommand("Удаление награды", TaskDecorator.Sync(RemoveCompetitionReward), IsAuthed);
        }
    }
}
