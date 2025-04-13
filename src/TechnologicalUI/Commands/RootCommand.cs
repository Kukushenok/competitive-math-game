using CompetitiveBackend.BackendUsage.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUI.Command;
using TechnologicalUI.MenuCommand;

namespace TechnologicalUI.Commands
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
        public RootCommand(IAuthUseCase authUseCase, ISelfUseCase selfUseCase, 
            IPlayerProfileUseCase playerProfileUseCase, IAuthCache authCache,
            ICompetitionEditUseCase competitionEditUseCase, ICompetitionWatchUseCase competitionWatchUseCase,
            IRewardDescriptionEditUseCase rewardDescriptionEditUseCase, IRewardDescriptionWatchUseCase rewardDescriptionWatchUseCase,
            ICompetitionRewardEditUseCase competitionRewardEditUseCase, IPlayerRewardUseCase playerReward,
            IPlayerParticipationUseCase playerParticipationUseCase, IPlayerParticipationWatchUseCase playerParticipationWatchUseCase)
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
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return authCommands;
            yield return Sum("Игрок", playerUseCaseCommands, selfPlayerCommands, playerRewardUseCase);
            yield return Sum("Соревнования", Join("Просмотр соревнований", competitionWatchCommands, playerParticipationWatchCommands), competitionEditCommands, competitionRewardEditCommands, playerParticipationUseCaseCommands);
            yield return Sum("Награды", rewardDescriptionWatchCommands, rewardDescriptionEditCommands);
        }
    }
}
