using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface IPlayerProfileUseCase
    {
        Task<PlayerProfileDTO> GetProfile(int id);
        Task<LargeDataDTO> GetProfileImage(int id);
    }
}
