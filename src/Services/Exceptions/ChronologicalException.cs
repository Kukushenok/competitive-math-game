namespace CompetitiveBackend.Services.Exceptions
{
    public class ChronologicalException : ServiceException
    {
        public ChronologicalException()
        {
        }

        public ChronologicalException(string message)
            : base(message)
        {
        }

        public ChronologicalException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
