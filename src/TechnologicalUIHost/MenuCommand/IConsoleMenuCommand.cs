using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public interface IConsoleMenuCommand
    {
        public Task Execute(IConsole console);
        public string GetLabel();
        public bool Enabled { get; }
    }
}
