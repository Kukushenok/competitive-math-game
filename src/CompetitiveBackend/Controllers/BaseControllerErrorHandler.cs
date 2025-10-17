using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CompetitiveBackend.Controllers
{
    public class BaseControllerErrorHandler: IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
        {
            IResult rslt = Results.Problem(statusCode: 500, detail: ex.Message, title: "Something's wrong with our side...");
            if (ex is RepositoryException)
            {
                if (ex is MissingDataException) rslt = Results.Problem(statusCode: 404, detail: ex.Message, title: "Not found");
                else if (ex is FailedOperationException) rslt = Results.Problem(statusCode: 400, detail: ex.Message, title: "Failed operation");
                else if (ex is IncorrectOperationException) rslt = Results.Problem(statusCode: 400, detail: ex.Message, title: "Incorrect operation");
            }
            else if (ex is ServiceException)
            {
                if (ex is BadImageException) rslt = Results.Problem(statusCode: 400, detail: ex.Message, title: "Bad image");
                else if (ex is IncorrectPasswordException) rslt = Results.Problem(statusCode: 400, detail: ex.Message);
                else if (ex is ConflictLoginException) rslt = Results.Problem(statusCode: 409, detail: ex.Message);
                else if (ex is BadLoginException) rslt = Results.Problem(statusCode: 400, detail: ex.Message);
                else if (ex is InvalidArgumentsException invx) rslt = Results.Problem(statusCode: 400, detail: invx.Message, title: "Invalid arguments");
            }
            else if (ex is UseCaseException)
            {
                if (ex is UnauthenticatedException) rslt = Results.Problem(statusCode: 401, detail: ex.Message, title: "Unauthorized");
                else if (ex is OperationNotPermittedException) rslt = Results.Problem(statusCode: 403, detail: ex.Message, title: "Forbidden");
                else if (ex is IsNotPlayerException) rslt = Results.Problem(statusCode: 403, detail: ex.Message, title: "Forbidden");
                else if (ex is RequestFailedException) rslt = Results.Problem(statusCode: 500, detail: ex.Message, title: "Something's wrong with our side...");
            }
            await rslt.ExecuteAsync(httpContext);
            return httpContext.Response.StatusCode != 500;
        }
    }
}
