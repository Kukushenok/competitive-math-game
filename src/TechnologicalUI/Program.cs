using CompetitiveBackend.SolutionInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechnologicalUI.Commands;
using TechnologicalUI.MenuCommand;

namespace TechnologicalUI
{
    internal class ProgramHost : IHost
    {
        public ProgramHost(IServiceProvider provider)
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
            RootCommand cmd = Services.GetService<RootCommand>()!;
            await cmd.Execute(new ConsoleInOut());
            await StopAsync();
            Environment.Exit(0);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddLogging(conf => conf.AddConsole());
            builder.Services.AddCompetitiveBackendSolution();
            builder.Services.AddSingleton<RootCommand>();
            builder.Services.AddSingleton<IAuthCache, AuthCache>();
            builder.Services.AddSingleton<IHost, ProgramHost>();
            builder.Build().Run();
        }
    }
}
