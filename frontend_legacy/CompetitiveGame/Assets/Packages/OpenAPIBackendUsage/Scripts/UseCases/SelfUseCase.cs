using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveFrontend.OpenAPIClient.Api;
using CompetitiveFrontend.OpenAPIClient.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendUsage.OpenAPI
{
    public abstract class AuthableUseCase<I> : IAuthableUseCase<I> where I : class, IAuthableUseCase<I>
    {
        protected abstract IReadableConfiguration Configuration { get; set; }
        public Task<I> Auth(string token)
        {
            IReadableConfiguration conf = GlobalConfiguration.MergeConfigurations(Configuration,
                new GlobalConfiguration(null, new Dictionary<string, string>() { { "Bearer", token } }, null));
            Configuration = conf;
            return Task.FromResult(this as I);
        }

        public void Dispose()
        {
            
        }
    }
    public class SelfUseCase : AuthableUseCase<ISelfUseCase>, ISelfUseCase
    {
        private ISelfPlayerApi selfPlayerApi;
        public SelfUseCase(ISelfPlayerApi selfPlayerApi)
        {
            this.selfPlayerApi = selfPlayerApi;
        }

        protected override IReadableConfiguration Configuration { get => selfPlayerApi.Configuration; set => selfPlayerApi.Configuration = value; }

        public async Task<LargeDataDTO> GetMyImage()
        {
            var q = await selfPlayerApi.ApiV1PlayerSelfImageGetWithHttpInfoAsync();
            
            return new LargeDataDTO(q.Data as byte[] ?? new byte[0]);
            //return ;
        }

        public async Task<PlayerProfileDTO> GetMyProfile() => await selfPlayerApi.ApiV1PlayerSelfGetAsync();

        public async Task UpdateMyImage(LargeDataDTO data)
        {
            await selfPlayerApi.ApiV1PlayerSelfImagePostAsync(new MemoryStream(data.Data));
        }

        public async Task UpdateMyPlayerProfile(PlayerProfileDTO p)
        {
            await selfPlayerApi.ApiV1PlayerSelfPatchAsync(p);
        }
    }
}
