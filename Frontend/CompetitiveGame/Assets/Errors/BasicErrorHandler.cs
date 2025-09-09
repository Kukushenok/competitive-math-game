using CompetitiveFrontend.OpenAPIClient.Client;
using System;

public class BasicErrorHandler
{
    private IErrorMessagesConfiguration configuration;
    private IUserMessageLogger userMessageLogger;
    public BasicErrorHandler(IErrorMessagesConfiguration configuration, IUserMessageLogger userMessageLogger)
    {
        this.configuration = configuration;
        this.userMessageLogger = userMessageLogger;
    }

    public Exception CreateException(string methodName, IApiResponse response)
    {
        Exception generated = new CompetitiveBackend.BackendUsage.Exceptions.UseCaseException();
        string message = configuration.ServerErrorMessage + response.ErrorText;

        switch (response.StatusCode)
        {
            case System.Net.HttpStatusCode.Forbidden:
                message = configuration.NotPermittedMessage;
                generated = new CompetitiveBackend.BackendUsage.Exceptions.OperationNotPermittedException();
                break;
            case System.Net.HttpStatusCode.Unauthorized:
                message = configuration.NotAuthorizedMessage;
                generated = new CompetitiveBackend.BackendUsage.Exceptions.UnauthenticatedException();
                break;
            case System.Net.HttpStatusCode.BadRequest:
                message = configuration.BadRequestMessage + response.ErrorText;
                generated = new CompetitiveBackend.BackendUsage.Exceptions.RequestFailedException();
                break;
        }
        userMessageLogger.LogError(message);
        return generated;
    }
}
