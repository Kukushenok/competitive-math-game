using System;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class OperationNotPermittedException : RequestFailedException
    {
        // public OperationNotPermittedException() { }
        public OperationNotPermittedException(string message = "You have no rights to use this")
            : base(message)
        {
        }

        public OperationNotPermittedException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
