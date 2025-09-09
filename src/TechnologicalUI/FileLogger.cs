using Microsoft.Extensions.Logging;

namespace TechnologicalUI
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;

        public FileLoggerProvider(string filePath)
        {
            _filePath = filePath;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName, _filePath);
        }

        public void Dispose()
        {
            // Очистка ресурсов при необходимости
        }
    }
    public class FileLogger : ILogger
    {
        private readonly string _filePath;

        public FileLogger(string path)
        {
            _filePath = path;
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        }

        public IDisposable BeginScope<TState>(TState state) => null!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
                              Exception exception, Func<TState, Exception, string> formatter)
        {
            var message = formatter(state, exception);
            File.AppendAllText(_filePath, $"{DateTime.Now} [{logLevel}] {message}\n");
        }
    }
}
