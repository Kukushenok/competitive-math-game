using System.Runtime.Serialization;

namespace CompetitiveBackend.Core.Exceptions.Repository
{
    public class RepositoryException: Exception
    {
        public RepositoryException() {}
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception? innerException) : base(message, innerException) { }
        protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    public class IncorrectOperationException: RepositoryException
    {
        public IncorrectOperationException() { }
        public IncorrectOperationException(string message) : base(message) { }
        public IncorrectOperationException(string message, Exception? innerException) : base(message, innerException) { }
        protected IncorrectOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    public class MissingDataException:RepositoryException
    {
        public MissingDataException() { }
        public MissingDataException(string message) : base(message) { }
        public MissingDataException(string message, Exception? innerException) : base(message, innerException) { }
        protected MissingDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
    public class FailedOperationException:RepositoryException
    {
        public FailedOperationException() { }
        public FailedOperationException(string message) : base(message) { }
        public FailedOperationException(string message, Exception? innerException) : base(message, innerException) { }
        protected FailedOperationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
