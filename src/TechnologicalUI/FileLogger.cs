using Microsoft.Extensions.Logging;

namespace TechnologicalUI
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string filePath;

        public FileLoggerProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(filePath);
        }

        public void Dispose()
        {
            // Очистка ресурсов при необходимости
        }
    }

    public class FileLogger : ILogger
    {
        private readonly string filePath;

        public FileLogger(string path)
        {
            filePath = path;
            string? d = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(d))
            {
                Directory.CreateDirectory(d);
            }
        }

        public IDisposable BeginScope<TState>(TState state)
            where TState : notnull
        {
            return null!;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            string message = formatter(state, exception);
            File.AppendAllText(filePath, $"{DateTime.Now} [{logLevel}] {message}\n");
        }
    }
}
