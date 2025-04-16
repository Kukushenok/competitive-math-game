using System;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class RequestFailedException: UseCaseException
    {
        public RequestFailedException() { }
        public RequestFailedException(string message) : base(message) { }
        public RequestFailedException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
