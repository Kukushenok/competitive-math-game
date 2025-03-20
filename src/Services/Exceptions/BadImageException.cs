namespace CompetitiveBackend.Services.Exceptions
{
    public class BadImageException : ServiceException
    {
        public BadImageException() { }
        public BadImageException(string message) : base(message) { }
        public BadImageException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
