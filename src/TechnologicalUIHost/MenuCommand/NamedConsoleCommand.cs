using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public abstract class NamedConsoleCommand : IConsoleMenuCommand
    {
        protected string name;
        public NamedConsoleCommand(string name)
        {
            this.name = name;
        }

        public abstract Task Execute(IConsole console);

        public string GetLabel()
        {
            return name;
        }

        public virtual bool Enabled => true;
    }
}
