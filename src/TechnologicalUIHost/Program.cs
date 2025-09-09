using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechnologicalUIHost.Commands;
using TechnologicalUIHost.MenuCommand;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost
{
    internal class TechnologicalUIHost : IHost
    {
        public TechnologicalUIHost(IServiceProvider provider)
        {
            Services = provider;
        }
        public IServiceProvider Services;

        IServiceProvider IHost.Services => Services;

        public void Dispose()
        {
             
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            RootCommand cmd = Services.GetRequiredService<RootCommand>();
            await cmd.Execute(Services.GetRequiredService<IConsole>());
            await StopAsync();
            Environment.Exit(0);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
