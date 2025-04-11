namespace TechnologicalUI.Command
{
    public interface IConsoleMenuCommand
    {
        public Task Execute();
        public string GetLabel();
        public bool Enabled { get; }
    }
}
