using System.Runtime.Serialization;

namespace CompetitiveBackend.Repositories.Exceptions
{
    public class FailedOperationException:RepositoryException
    {
        public FailedOperationException() { }
        public FailedOperationException(string message) : base(message) { }
        public FailedOperationException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
