using CompetitiveBackend.BackendUsage.Objects;

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
