namespace CompetitiveBackend.Services.Exceptions
{
    public class ConflictLoginException : ServiceException
    {
        public ConflictLoginException()
        {
        }

        public ConflictLoginException(string message)
            : base(message)
        {
        }

        public ConflictLoginException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
