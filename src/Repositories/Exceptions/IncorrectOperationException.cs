namespace CompetitiveBackend.Repositories.Exceptions
{
    public class IncorrectOperationException : RepositoryException
    {
        public IncorrectOperationException() { }
        public IncorrectOperationException(string message) : base(message) { }
        public IncorrectOperationException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
