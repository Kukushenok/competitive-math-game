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
            IResult rslt = Results.Problem(statusCode: 500, detail: "Unknown exception was thrown: " + ex.Message);
            if (ex is RepositoryException)
            {
                if (ex is MissingDataException) rslt = Results.NotFound();
                else if (ex is FailedOperationException) rslt = Results.BadRequest();
                else if (ex is IncorrectOperationException) rslt = Results.BadRequest();
            }
            else if (ex is ServiceException)
            {
                if (ex is BadImageException) rslt = Results.BadRequest("Image is not sufficient");
                else if (ex is IncorrectPasswordException) rslt = Results.Unauthorized();
                else if (ex is BadLoginException) rslt = Results.BadRequest("Login is erroneous");
                else if (ex is InvalidArgumentsException invx) rslt = Results.BadRequest($"Some field is invalid: {invx.Message}");
            }
            else if (ex is UseCaseException)
            {
                if (ex is UnauthenticatedException) rslt = Results.Unauthorized();
                else if (ex is OperationNotPermittedException) rslt = Results.Forbid();
                else if (ex is IsNotPlayerException) rslt = Results.Forbid();
                else if (ex is RequestFailedException) rslt = Results.StatusCode(500);
            }
            await rslt.ExecuteAsync(httpContext);
            return httpContext.Response.StatusCode != 500;
        }
    }
}
