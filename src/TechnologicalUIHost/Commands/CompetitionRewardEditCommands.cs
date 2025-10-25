using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class CompetitionRewardEditCommands : AuthRequiringCommands
    {
        private readonly ICompetitionRewardEditUseCase rewardEditUseCase;

        public CompetitionRewardEditCommands(IAuthCache cache, ICompetitionRewardEditUseCase useCase)
            : base("Изменение наград за соревнования", cache)
        {
            rewardEditUseCase = useCase;
        }

        private async Task CreateCompetitionReward(IConsole console)
        {
            using ICompetitionRewardEditUseCase self = await Auth(rewardEditUseCase);
            self.CreateCompetitionReward(console.ReadCreateCompetitionRewardDO()).GetAwaiter().GetResult();
        }

        private async Task UpdateCompetitionReward(IConsole console)
        {
            using ICompetitionRewardEditUseCase self = await Auth(rewardEditUseCase);
            self.UpdateCompetitionReward(console.ReadUpdateCompetitionRewardDTO()).GetAwaiter().GetResult();
        }

        private async Task RemoveCompetitionReward(IConsole console)
        {
            using ICompetitionRewardEditUseCase self = await Auth(rewardEditUseCase);
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
