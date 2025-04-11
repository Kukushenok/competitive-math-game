using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalUI.MenuCommand
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
        public void Login(AuthSuccessResultDTO dto)
        {
            Console.WriteLine($"Authenticated as {dto.RoleName}; playerID {dto.AccountID}");
            resultDTO = dto;
        }
        public bool IsAuthed() => resultDTO != null;
        public void LogOff() { resultDTO = null; }
        public string GetToken() => resultDTO?.Token ?? "";
    }
    
}
