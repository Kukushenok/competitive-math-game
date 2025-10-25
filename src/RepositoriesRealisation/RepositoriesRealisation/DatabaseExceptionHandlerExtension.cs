using Microsoft.EntityFrameworkCore;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    internal static class DatabaseExceptionHandlerExtension
    {
        public static bool IsDBException(this Exception ex)
        {
            return ex is OperationCanceledException or
                                       DbUpdateException or
                                       DbUpdateConcurrencyException;
        }
    }
}
