using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public abstract class NamedConsoleCommand: IConsoleMenuCommand
    {
        protected string Name;
        public NamedConsoleCommand(string name)
        {
            Name = name;
        }

        public abstract Task Execute(IConsole console);

        public string GetLabel() => Name;
        public virtual bool Enabled { get => true; }
    }
}
