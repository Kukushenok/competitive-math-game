using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
namespace XUnitLoggingProvider
{
    internal class XUnitLogger : ILogger
    {
        class DummyScope : IDisposable
        {
            public void Dispose()
            {

            }
        }
        ITestOutputHelper output;
        string? category;
        public XUnitLogger(ITestOutputHelper output, string? category = null)
        {
            this.output = output;
            this.category = category;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            output.WriteLine("Got into " + state.ToString());
            return new DummyScope();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string form = formatter(state, exception);
            output.WriteLine((category ?? "") + eventId.ToString() + " : " + form);
        }
    }
    internal class XUnitFactory(ITestOutputHelper output) : ILoggerFactory, ILoggerProvider
    {
        public void AddProvider(ILoggerProvider provider)
        {
            return;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(output, categoryName);
        }

        public void Dispose()
        {

        }
    }
}