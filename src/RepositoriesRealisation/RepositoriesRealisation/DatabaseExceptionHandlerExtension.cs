using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.RepositoriesRealisation
{
    internal static class DatabaseExceptionHandlerExtension
    {
        public static bool IsDBException(this Exception ex)
        {
            return (ex is OperationCanceledException ||
                                       ex is DbUpdateException ||
                                       ex is DbUpdateConcurrencyException);
        }
    }
}
