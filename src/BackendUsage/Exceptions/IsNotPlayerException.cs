using System;

namespace CompetitiveBackend.BackendUsage.Exceptions
{
    public class IsNotPlayerException: RequestFailedException
    {
        public IsNotPlayerException(string message = "Only player may do this") : base(message) { }
        public IsNotPlayerException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
