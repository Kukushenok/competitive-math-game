using System;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class UnauthenticatedException: RequestFailedException
    {
        //public UnauthenticatedException() { }
        public UnauthenticatedException(string message = "To use this, you should authenticate first") : base(message) { }
        public UnauthenticatedException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
