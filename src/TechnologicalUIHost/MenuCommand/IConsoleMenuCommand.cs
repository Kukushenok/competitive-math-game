using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public interface IConsoleMenuCommand
    {
        Task Execute(IConsole console);
        string GetLabel();
        bool Enabled { get; }
    }
}
