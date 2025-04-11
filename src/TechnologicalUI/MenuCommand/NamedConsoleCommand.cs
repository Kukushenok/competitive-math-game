namespace TechnologicalUI.Command
{
    public abstract class NamedConsoleCommand: IConsoleMenuCommand
    {
        protected string Name;
        public NamedConsoleCommand(string name)
        {
            Name = name;
        }

        public abstract Task Execute();

        public string GetLabel() => Name;
        public virtual bool Enabled { get => true; }
    }
}
