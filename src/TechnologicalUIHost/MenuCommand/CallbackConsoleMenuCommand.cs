using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public class CallbackConsoleMenuCommand : NamedConsoleCommand
    {
        private readonly Action<IConsole> action;
        private readonly Func<bool>? enabled;
        public override bool Enabled => enabled == null || enabled();

        public CallbackConsoleMenuCommand(string name, Action<IConsole> action, Func<bool>? enabled = null)
            : base(name)
        {
            this.action = action;
            this.enabled = enabled;
        }

        public override async Task Execute(IConsole console)
        {
            await Task.Run(() => action(console));
        }
    }
}
