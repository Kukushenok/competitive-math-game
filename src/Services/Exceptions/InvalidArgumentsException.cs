namespace CompetitiveBackend.Services.Exceptions
{
    public class InvalidArgumentsException : ServiceException
    {
        public InvalidArgumentsException() { }
        public InvalidArgumentsException(string message) : base(message) { }
        public InvalidArgumentsException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
