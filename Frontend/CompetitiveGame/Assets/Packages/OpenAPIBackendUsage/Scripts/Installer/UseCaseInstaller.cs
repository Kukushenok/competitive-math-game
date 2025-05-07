using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveFrontend.OpenAPIClient.Api;
using CompetitiveFrontend.OpenAPIClient.Client;
using UnityEngine;
using VContainer;

namespace BackendUsage.OpenAPI
{
    public interface IAuthCache
    {
        public string AuthToken { get; set; }
    }
    public static class UseCaseInstaller
    {
        public static IContainerBuilder InstallRemoteUseCases(this IContainerBuilder container, IReadableConfiguration conf = null)
        {
            container.InstallOpenAPI(conf);
            container.InstallUseCases();
            return container;
        }
        public static IContainerBuilder InstallOpenAPI(this IContainerBuilder container, IReadableConfiguration conf = null)
        {
            container.Register(x=>new ApiClient(conf.BasePath, x.ResolveOrDefault(Configuration.DefaultExceptionFactory)), Lifetime.Singleton)
                .As<ISynchronousClient, IAsynchronousClient>();
            container.Register<IReadableConfiguration>((IObjectResolver rslv) => new GlobalConfigurationSpoofer(conf, rslv.Resolve<IAuthCache>()), Lifetime.Singleton);
            container.Register<IAuthorizationApi>((IObjectResolver rslv) =>
            {
                var x = new AuthorizationApi(rslv.Resolve<ISynchronousClient>(),
                rslv.Resolve<IAsynchronousClient>(), rslv.Resolve<IReadableConfiguration>());
                x.ExceptionFactory = rslv.ResolveOrDefault(Configuration.DefaultExceptionFactory);
                return x;
            }, Lifetime.Singleton);
            container.Register<ISelfPlayerApi>((IObjectResolver rslv) => {
                var x = new SelfPlayerApi(rslv.Resolve<ISynchronousClient>(),
                    rslv.Resolve<IAsynchronousClient>(), rslv.Resolve<IReadableConfiguration>());
                x.ExceptionFactory = rslv.ResolveOrDefault(Configuration.DefaultExceptionFactory);
                return x;
                }, Lifetime.Singleton);
            
            return container;
        }
        public static IContainerBuilder InstallUseCases(this IContainerBuilder container)
        {
            container.Register<IAuthUseCase, AuthUseCase>(Lifetime.Singleton);
            container.Register<ISelfUseCase, SelfUseCase>(Lifetime.Singleton);
            return container;
        }
    }
}
