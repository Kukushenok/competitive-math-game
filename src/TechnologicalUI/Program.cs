using ClientUsage.Installer;
//using CompetitiveBackend.SolutionInstaller;
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
            bool local = Decide(builder.Services);
            builder.Services.AddTechnologicalUIHostWithConsole<ConsoleInOut>();
            IHost hst = builder.Build();
            //if(local) hst.Services.InitializeCompetitiveBackendSolution();
            hst.Run();
        }
        static bool Decide(IServiceCollection coll)
        {
            Console.Write("Введите HTTP-адрес сервера (по умолчанию http://localhost:8080/). (Напишите host, чтобы не использовать): ");
            var input = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                input = "http://localhost:8080/";
            }
            //{
            //    coll.AddCompetitiveBackendSolution();
            //} 
            //else
            //{
            coll.AddRemoteUseCases(input);
            //}
            return false;
        }
    }
}
