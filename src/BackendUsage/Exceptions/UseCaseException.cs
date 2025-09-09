using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class UseCaseException: Exception
    {
        public UseCaseException() { }
        public UseCaseException(string message) : base(message) { }
        public UseCaseException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
