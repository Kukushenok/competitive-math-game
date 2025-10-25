using ClientUsage.Client;
using ClientUsage.Objects;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;

namespace ClientUsage.UseCases
{
    internal sealed class SelfUseCase : AuthableUseCaseBase<ISelfUseCase>, ISelfUseCase
    {
        public SelfUseCase(IHttpClient client)
            : base(client)
        {
        }

        public override Task<ISelfUseCase> Auth(string token)
        {
            IHttpClient authClient = CreateAuthClient(token);
            ISelfUseCase impl = new SelfUseCase(authClient);
            return Task.FromResult(impl);
        }

        public Task UpdateMyImage(LargeDataDTO data)
        {
            using var ms = new MemoryStream(data.Data ?? []);
            return client.PostMultipartNoContent("/api/v1/players/me/image", ms, "file", "file.bin", "application/octet-stream");
        }

        public Task<PlayerProfileDTO> GetMyProfile()
        {
            return client.Get<PlayerProfileDTO>("/api/v1/players/me/profile");
        }

        public Task<LargeDataDTO> GetMyImage()
        {
            return client.Get<LargeDataDTO>("/api/v1/players/me/image");
        }

        public Task UpdateMyPlayerProfile(PlayerProfileDTO p)
        {
            return client.PutNoContent("/api/v1/players/me/profile", p);
        }
    }
}
