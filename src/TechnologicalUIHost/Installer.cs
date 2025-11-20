using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TechnologicalUIHost.Commands;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUIHost
{
    public static class Installer
    {
        public static IServiceCollection AddTechnologicalUIHostWithConsole<T>(this IServiceCollection collection)
            where T : class, IConsole
        {
            collection.AddSingleton<RootCommand>();
            collection.AddSingleton<IHost, TechnologicalUIHost>();
            collection.AddSingleton<IConsole, T>();
            collection.AddSingleton<IAuthCache, AuthCache>();
            return collection;
        }
    }
}
