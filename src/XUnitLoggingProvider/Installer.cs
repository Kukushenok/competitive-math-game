using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
namespace XUnitLoggingProvider
{
    public static class Installer
    {
        public static IServiceCollection UseXUnitLogging(this IServiceCollection coll, ITestOutputHelper helper)
        {
            var factory = new XUnitFactory(helper);
            coll.AddSingleton<ILoggerFactory>(factory);
            coll.AddSingleton<ILogger>(new XUnitLogger(helper));
            coll.AddSingleton<ILoggerProvider>(factory);
            return coll;
        }
    }
}