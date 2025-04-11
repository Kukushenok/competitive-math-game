using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage
{
    public interface IAuthUseCase
    {
        public Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation);
        public Task<AuthSuccessResultDTO> Login(string login, string password);
    }
    public interface ISelfUseCase: IAuthableUseCase<ISelfUseCase>
    {
        public Task UpdateMyImage(LargeData data);
        public Task<PlayerProfile> GetMyProfile();
        public Task<LargeData> GetMyImage();
        public Task UpdateMyPlayerProfile(PlayerProfile p);
    }
    public interface IPlayerProfileUseCase
    {
        public Task<PlayerProfile> GetProfile(int id);
        public Task<LargeData> GetProfileImage(int id);
    }
}
