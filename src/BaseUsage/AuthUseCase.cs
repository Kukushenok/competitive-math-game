using CompetitiveBackend.BackendUsage;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage
{
    public class AuthUseCase : IAuthUseCase
    {
        private IAuthService authService;
        public AuthUseCase(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<AuthSuccessResultDTO> Login(string login, string password)
        {
            var result = await authService.LogIn(login, password);
            return new AuthSuccessResultDTO(result.Token, result.RoleName, result.AccountID);
        }

        public async Task<AuthSuccessResultDTO> Register(AccountCreationDTO creationDTO)
        {
            await authService.Register(new Core.Objects.Account(creationDTO.Login, null!, creationDTO.Email), creationDTO.Password);
            return await Login(creationDTO.Login, creationDTO.Password);
        }
    }
}
