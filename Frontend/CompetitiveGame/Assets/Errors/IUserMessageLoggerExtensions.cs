public static class IUserMessageLoggerExtensions
{
    public static void LogInfo(this IUserMessageLogger logger, string message) => logger.Log(IUserMessageLogger.LogLevel.Normal, message);
    public static void LogError(this IUserMessageLogger logger, string message) => logger.Log(IUserMessageLogger.LogLevel.Error, message);
}
