using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.MenuCommand
{
    public interface IAuthCache
    {
        public void Login(AuthSuccessResultDTO dto);
        bool IsAuthed();
        void LogOff();
        string GetToken();
    }
    public class AuthCache: IAuthCache
    {
        private AuthSuccessResultDTO? resultDTO = null;
        private IConsole console;
        public AuthCache(IConsole console)
        {
            this.console = console;
        }
        public void Login(AuthSuccessResultDTO dto)
        {
            console.PromtOutput($"Выполнен вход как {dto.RoleName}; ID игрока {dto.AccountID}");
            resultDTO = dto;
        }
        public bool IsAuthed() => resultDTO != null;
        public void LogOff() { resultDTO = null; }
        public string GetToken() => resultDTO?.Token ?? "";
    }
    
}
