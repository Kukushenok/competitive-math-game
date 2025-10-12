using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUIHost.Command;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost.Commands
{
    class RootCommand: CompositeCommandBlock
    {
        private AuthCommands authCommands;
        private SelfPlayerCommands selfPlayerCommands;
        private PlayerUseCaseCommands playerUseCaseCommands;
        private CompetitionWatchCommands competitionWatchCommands;
        private CompetitionEditCommands competitionEditCommands;
        private RewardDescriptionEditCommands rewardDescriptionEditCommands;
        private RewardDescriptionWatchCommands rewardDescriptionWatchCommands;
        private CompetitionRewardEditCommands competitionRewardEditCommands;
        private PlayerRewardUseCaseCommands playerRewardUseCase;
        private PlayerParticipationUseCaseCommands playerParticipationUseCaseCommands;
        private PlayerParticipationWatchUseCaseCommands playerParticipationWatchCommands;
        private GameParticipationUseCaseCommands gameUseCaseCommands;
        private GameManagementUseCaseCommands managementUseCaseCommands;
        public RootCommand(IAuthUseCase authUseCase, ISelfUseCase selfUseCase, 
            IPlayerProfileUseCase playerProfileUseCase, IAuthCache authCache,
            ICompetitionEditUseCase competitionEditUseCase, ICompetitionWatchUseCase competitionWatchUseCase,
            IRewardDescriptionEditUseCase rewardDescriptionEditUseCase, IRewardDescriptionWatchUseCase rewardDescriptionWatchUseCase,
            ICompetitionRewardEditUseCase competitionRewardEditUseCase, IPlayerRewardUseCase playerReward,
            IPlayerParticipationUseCase playerParticipationUseCase, IPlayerParticipationWatchUseCase playerParticipationWatchUseCase,
            IGamePlayUseCase gamePlayUseCase, IGameManagementUseCase gameManagementUseCase)
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
            yield return Sum("Соревнования", 
                Join("Просмотр соревнований", competitionWatchCommands, playerParticipationWatchCommands), 
                Join("Редактирование", competitionEditCommands, managementUseCaseCommands), 
                competitionRewardEditCommands, 
                playerParticipationUseCaseCommands,
                gameUseCaseCommands);
            yield return Sum("Награды", rewardDescriptionWatchCommands, rewardDescriptionEditCommands);
        }
    }
}
