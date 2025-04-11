using CompetitiveBackend.BackendUsage;
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
        public RootCommand(IAuthUseCase authUseCase, ISelfUseCase selfUseCase, IPlayerProfileUseCase playerProfileUseCase, IAuthCache authCache) : base("Основной интерфеейс")
        {
            authCommands = new AuthCommands(authCache, authUseCase);
            selfPlayerCommands = new SelfPlayerCommands(selfUseCase, authCache);
            playerUseCaseCommands = new PlayerUseCaseCommands(playerProfileUseCase);
        }
        protected override IEnumerable<IConsoleMenuCommand> GetCommands()
        {
            yield return authCommands;
            yield return selfPlayerCommands;
            yield return playerUseCaseCommands;
        }
    }
}
