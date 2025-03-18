namespace CompetitiveBackend.Services.Exceptions
{
    public class IncorrectPasswordException : ServiceException
    {
        public IncorrectPasswordException() { }
        public IncorrectPasswordException(string message) : base(message) { }
        public IncorrectPasswordException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
