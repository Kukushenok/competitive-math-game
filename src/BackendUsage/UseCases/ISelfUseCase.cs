using CompetitiveBackend.BackendUsage.Objects;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.UseCases
{
    public interface ISelfUseCase: IAuthableUseCase<ISelfUseCase>
    {
        public Task UpdateMyImage(LargeDataDTO data);
        public Task<PlayerProfileDTO> GetMyProfile();
        public Task<LargeDataDTO> GetMyImage();
        public Task UpdateMyPlayerProfile(PlayerProfileDTO p);
    }
}
