using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveBackend.Services;

namespace CompetitiveBackend.BaseUsage.UseCases
{
    public class AuthUseCase : IAuthUseCase
    {
        private readonly IAuthService authService;
        public AuthUseCase(IAuthService authService)
        {
            this.authService = authService;
        }

        public async Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest)
        {
            Services.Objects.AuthSuccessResult result = await authService.LogIn(loginRequest.Login, loginRequest.Password);
            return new AuthSuccessResultDTO(result.Token, result.RoleName, result.AccountID);
        }

        public async Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation)
        {
            await authService.Register(new Core.Objects.Account(creation.Login, creation.Email), creation.Password);
            return await Login(creation);
        }
    }
}
