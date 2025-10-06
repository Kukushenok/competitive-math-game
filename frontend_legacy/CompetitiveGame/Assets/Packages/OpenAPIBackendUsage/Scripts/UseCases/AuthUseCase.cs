using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveFrontend.OpenAPIClient.Api;
using System.Threading.Tasks;
using UnityEngine;

namespace BackendUsage.OpenAPI
{
    public class AuthUseCase : IAuthUseCase
    {
        private IAuthorizationApi authAPI;
        public AuthUseCase(IAuthorizationApi authorizationApi)
        {
            authAPI = authorizationApi;
        }
        public async Task<AuthSuccessResultDTO> Login(AccountLoginDTO loginRequest)
        {
            return await authAPI.ApiV1AuthLoginPostAsync(loginRequest);
            //
        }

        public async Task<AuthSuccessResultDTO> Register(AccountCreationDTO creation)
        {
            return await authAPI.ApiV1AuthRegisterPostAsync(creation);
        }
    }
}
