using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IAuthUseCase
    {
        Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation);
        Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest);
    }
}
