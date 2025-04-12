using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IAuthUseCase
    {
        public Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation);
        public Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest);
    }
}
