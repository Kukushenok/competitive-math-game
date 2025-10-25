using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CompetitiveBackend.Controllers
{
    public class BaseControllerErrorHandler : IExceptionHandler
    {
        private static IResult HandleException(RepositoryException exception)
        {
            IResult rslt = Results.Problem(statusCode: 500, detail: exception.Message, title: "Something's wrong with our side...");
            if (exception is MissingDataException)
            {
                rslt = Results.Problem(statusCode: 404, detail: exception.Message, title: "Not found");
            }
            else if (exception is FailedOperationException)
            {
                rslt = Results.Problem(statusCode: 400, detail: exception.Message, title: "Failed operation");
            }
            else if (exception is IncorrectOperationException)
            {
                rslt = Results.Problem(statusCode: 400, detail: exception.Message, title: "Incorrect operation");
            }

            return rslt;
        }

        private static IResult HandleException(ServiceException exception)
        {
            IResult rslt = Results.Problem(statusCode: 500, detail: exception.Message, title: "Something's wrong with our side...");
            if (exception is BadImageException)
            {
                rslt = Results.Problem(statusCode: 400, detail: exception.Message, title: "Bad image");
            }
            else if (exception is IncorrectPasswordException)
            {
                rslt = Results.Problem(statusCode: 400, detail: exception.Message);
            }
            else if (exception is ConflictLoginException)
            {
                rslt = Results.Problem(statusCode: 409, detail: exception.Message);
            }
            else if (exception is BadLoginException)
            {
                rslt = Results.Problem(statusCode: 400, detail: exception.Message);
            }
            else if (exception is InvalidArgumentsException invx)
            {
                rslt = Results.Problem(statusCode: 400, detail: invx.Message, title: "Invalid arguments");
            }

            return rslt;
        }

        private static IResult HandleException(UseCaseException exception)
        {
            IResult rslt = Results.Problem(statusCode: 500, detail: exception.Message, title: "Something's wrong with our side...");
            if (exception is UnauthenticatedException)
            {
                rslt = Results.Problem(statusCode: 401, detail: exception.Message, title: "Unauthorized");
            }
            else if (exception is OperationNotPermittedException)
            {
                rslt = Results.Problem(statusCode: 403, detail: exception.Message, title: "Forbidden");
            }
            else if (exception is IsNotPlayerException)
            {
                rslt = Results.Problem(statusCode: 403, detail: exception.Message, title: "Forbidden");
            }
            else if (exception is RequestFailedException)
            {
                rslt = Results.Problem(statusCode: 500, detail: exception.Message, title: "Something's wrong with our side...");
            }

            return rslt;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            IResult rslt = Results.Problem(statusCode: 500, detail: exception.Message, title: "Something's wrong with our side...");
            if (exception is RepositoryException rp)
            {
                rslt = HandleException(rp);
            }
            else if (exception is ServiceException se)
            {
                rslt = HandleException(se);
            }
            else if (exception is UseCaseException us)
            {
                rslt = HandleException(us);
            }

            await rslt.ExecuteAsync(httpContext);
            return httpContext.Response.StatusCode != 500;
        }
    }
}
