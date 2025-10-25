using System;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class InvalidInputDataException : UseCaseException
    {
        public InvalidInputDataException()
        {
        }

        public InvalidInputDataException(string message)
            : base(message)
        {
        }

        public InvalidInputDataException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
