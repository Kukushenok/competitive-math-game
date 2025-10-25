namespace CompetitiveBackend.Services.Exceptions
{
    public class GameSessionException : ServiceException
    {
        public GameSessionException()
        {
        }

        public GameSessionException(string message)
            : base(message)
        {
        }

        public GameSessionException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }

    public class GameSessionExpiredException : GameSessionException
    {
        public GameSessionExpiredException()
        {
        }

        public GameSessionExpiredException(string message)
            : base(message)
        {
        }

        public GameSessionExpiredException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }

    public class GameSessionInvalidException : GameSessionException
    {
        public GameSessionInvalidException()
        {
        }

        public GameSessionInvalidException(string message)
            : base(message)
        {
        }

        public GameSessionInvalidException(string message, Exception? innerException)
            : base(message, innerException)
        {
        }
    }
}
