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
    public class RequestFailedException: UseCaseException
    {
        public RequestFailedException() { }
        public RequestFailedException(string message) : base(message) { }
        public RequestFailedException(string message, Exception? innerException) : base(message, innerException) { }
    }
    public class OperationNotPermittedException: RequestFailedException
    {
        //public OperationNotPermittedException() { }
        public OperationNotPermittedException(string message = "You have no rights to use this") : base(message) { }
        public OperationNotPermittedException(string message, Exception? innerException) : base(message, innerException) { }
    }
    public class IsNotPlayerException: RequestFailedException
    {
        public IsNotPlayerException(string message = "Only player may do this") : base(message) { }
        public IsNotPlayerException(string message, Exception? innerException) : base(message, innerException) { }
    }
    public class UnauthenticatedException: RequestFailedException
    {
        //public UnauthenticatedException() { }
        public UnauthenticatedException(string message = "To use this, you should authenticate first") : base(message) { }
        public UnauthenticatedException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
