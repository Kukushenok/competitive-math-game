using CompetitiveBackend.BackendUsage.UseCases;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    internal sealed class RootCommand : CompositeCommandBlock
    {
        private readonly AuthCommands authCommands;
        private readonly SelfPlayerCommands selfPlayerCommands;
        private readonly PlayerUseCaseCommands playerUseCaseCommands;
        private readonly CompetitionWatchCommands competitionWatchCommands;
        private readonly CompetitionEditCommands competitionEditCommands;
        private readonly RewardDescriptionEditCommands rewardDescriptionEditCommands;
        private readonly RewardDescriptionWatchCommands rewardDescriptionWatchCommands;
        private readonly CompetitionRewardEditCommands competitionRewardEditCommands;
        private readonly PlayerRewardUseCaseCommands playerRewardUseCase;
        private readonly PlayerParticipationUseCaseCommands playerParticipationUseCaseCommands;
        private readonly PlayerParticipationWatchUseCaseCommands playerParticipationWatchCommands;
        private readonly GameParticipationUseCaseCommands gameUseCaseCommands;
        private readonly GameManagementUseCaseCommands managementUseCaseCommands;
        public RootCommand(
            IAuthUseCase authUseCase,
            ISelfUseCase selfUseCase,
            IPlayerProfileUseCase playerProfileUseCase,
            IAuthCache authCache,
            ICompetitionEditUseCase competitionEditUseCase,
            ICompetitionWatchUseCase competitionWatchUseCase,
            IRewardDescriptionEditUseCase rewardDescriptionEditUseCase,
            IRewardDescriptionWatchUseCase rewardDescriptionWatchUseCase,
            ICompetitionRewardEditUseCase competitionRewardEditUseCase,
            IPlayerRewardUseCase playerReward,
            IPlayerParticipationUseCase playerParticipationUseCase,
            IPlayerParticipationWatchUseCase playerParticipationWatchUseCase,
            IGamePlayUseCase gamePlayUseCase,
            IGameManagementUseCase gameManagementUseCase)
            : base("Основной интерфейс")
        {
            authCommands = new AuthCommands(authCache, authUseCase);
            selfPlayerCommands = new SelfPlayerCommands(selfUseCase, authCache);
            playerUseCaseCommands = new PlayerUseCaseCommands(playerProfileUseCase);
            competitionWatchCommands = new CompetitionWatchCommands(competitionWatchUseCase);
            competitionEditCommands = new CompetitionEditCommands(authCache, competitionEditUseCase);
            rewardDescriptionEditCommands = new RewardDescriptionEditCommands(authCache, rewardDescriptionEditUseCase);
            rewardDescriptionWatchCommands = new RewardDescriptionWatchCommands(rewardDescriptionWatchUseCase);
            competitionRewardEditCommands = new CompetitionRewardEditCommands(authCache, competitionRewardEditUseCase);
            playerRewardUseCase = new PlayerRewardUseCaseCommands(authCache, playerReward);
            playerParticipationUseCaseCommands = new PlayerParticipationUseCaseCommands(authCache, playerParticipationUseCase);
            playerParticipationWatchCommands = new PlayerParticipationWatchUseCaseCommands(playerParticipationWatchUseCase);
            gameUseCaseCommands = new GameParticipationUseCaseCommands(gamePlayUseCase, authCache);
            managementUseCaseCommands = new GameManagementUseCaseCommands(gameManagementUseCase, authCache);
        }

        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return authCommands;
            yield return Sum("Игрок", playerUseCaseCommands, selfPlayerCommands, playerRewardUseCase);
            yield return Sum(
                "Соревнования",
                Join("Просмотр соревнований", competitionWatchCommands, playerParticipationWatchCommands),
                Join("Редактирование", competitionEditCommands, managementUseCaseCommands),
                competitionRewardEditCommands,
                playerParticipationUseCaseCommands,
                gameUseCaseCommands);
            yield return Sum("Награды", rewardDescriptionWatchCommands, rewardDescriptionEditCommands);
        }
    }
}
