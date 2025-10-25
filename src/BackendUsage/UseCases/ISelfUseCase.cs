using System.Threading.Tasks;
using CompetitiveBackend.BackendUsage.Objects;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ISelfUseCase : IAuthableUseCase<ISelfUseCase>
    {
        Task UpdateMyImage(LargeDataDTO data);
        Task<PlayerProfileDTO> GetMyProfile();
        Task<LargeDataDTO> GetMyImage();
        Task UpdateMyPlayerProfile(PlayerProfileDTO p);
    }
}
