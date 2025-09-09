using CompetitiveBackend.SolutionInstaller;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TechnologicalUIHost;
using TechnologicalUIHost.Commands;
using TechnologicalUIHost.ConsoleAbstractions;
using TechnologicalUIHost.MenuCommand;

namespace TechnologicalUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddLogging(conf => conf.AddProvider(new FileLoggerProvider("log.log")));
            builder.Services.AddCompetitiveBackendSolution();
            builder.Services.AddTechnologicalUIHostWithConsole<ConsoleInOut>();
            IHost hst = builder.Build();
            hst.Services.InitializeCompetitiveBackendSolution();
            hst.Run();
        }
    }
}
