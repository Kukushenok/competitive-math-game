using CompetitiveBackend.BackendUsage.Objects;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.MenuCommand
{
    public interface IAuthCache
    {
        void Login(AuthSuccessResultDTO dto);
        bool IsAuthed();
        void LogOff();
        string GetToken();
    }

    public class AuthCache : IAuthCache
    {
        private readonly IConsole console;
        private AuthSuccessResultDTO? resultDTO;
        public AuthCache(IConsole console)
        {
            this.console = console;
        }

        public void Login(AuthSuccessResultDTO dto)
        {
            console.PromtOutput($"Выполнен вход как {dto.RoleName}; ID игрока {dto.AccountID}");
            resultDTO = dto;
        }

        public bool IsAuthed()
        {
            return resultDTO != null;
        }

        public void LogOff()
        {
            resultDTO = null;
        }

        public string GetToken()
        {
            return resultDTO?.Token ?? string.Empty;
        }
    }
}
