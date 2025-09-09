public interface IUserMessageLogger
{
    public enum LogLevel { Normal, Error}
    public void Log(LogLevel level, string message);
}
