namespace CompetitiveBackend.Repositories.Exceptions
{
    public class MissingDataException : RepositoryException
    {
        public MissingDataException() { }
        public MissingDataException(string message) : base(message) { }
        public MissingDataException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
