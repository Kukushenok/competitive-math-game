using CompetitiveFrontend.OpenAPIClient.Client;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ErrorLoggerLifetimeScope : LifetimeScope
{
    [SerializeField] private ErrorMessagesConstantConfiguration constantConfiguration;
    [SerializeField] private GameObject coolMessageLogger;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance<IErrorMessagesConfiguration>(constantConfiguration);
        builder.Register(x => x.Instantiate(coolMessageLogger).GetComponent<IUserMessageLogger>(), Lifetime.Singleton);
        builder.Register<BasicErrorHandler>(Lifetime.Singleton);
        builder.Register<ExceptionFactory>(x => x.Resolve<BasicErrorHandler>().CreateException, Lifetime.Singleton);
    }
}