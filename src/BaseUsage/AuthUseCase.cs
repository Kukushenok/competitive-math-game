using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
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

        public async Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest)
        {
            var result = await authService.LogIn(loginRequest.Login, loginRequest.Password);
            return new AuthSuccessResultDTO(result.Token, result.RoleName, result.AccountID);
        }

        public async Task<AuthSuccessResultDTO> Register(AccountCreationDTO creationDTO)
        {
            await authService.Register(new Core.Objects.Account(creationDTO.Login, null!, creationDTO.Email), creationDTO.Password);
            return await Login(creationDTO);
        }
    }
}
