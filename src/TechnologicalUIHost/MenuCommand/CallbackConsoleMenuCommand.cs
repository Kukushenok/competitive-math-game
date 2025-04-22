using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TechnologicalUIHost.ConsoleAbstractions;

namespace TechnologicalUIHost.Command
{
    public class CallbackConsoleMenuCommand : NamedConsoleCommand
    {
        private Action<IConsole> action;
        private Func<bool>? enabled;
        public override bool Enabled 
        {
            get
            {
                if (enabled == null) return true;
                return enabled();
            }
        }
        public CallbackConsoleMenuCommand(string name, Action<IConsole> action, Func<bool>? Enabled = null) : base(name)
        {
            this.action = action;
            this.enabled = Enabled;
        }

        public override async Task Execute(IConsole console)
        {
            await Task.Run(() => action(console));
        }

    }
}
