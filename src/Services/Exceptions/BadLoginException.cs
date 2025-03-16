namespace CompetitiveBackend.Services.Exceptions
{
    public class BadLoginException : ServiceException
    {
        public BadLoginException() { }
        public BadLoginException(string message) : base(message) { }
        public BadLoginException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
