using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TechnologicalUI.Command
{
    public class CallbackConsoleMenuCommand : NamedConsoleCommand
    {
        private Action action;
        private Func<bool>? enabled;
        public override bool Enabled 
        {
            get
            {
                if (enabled == null) return true;
                return enabled();
            }
        }
        public CallbackConsoleMenuCommand(string name, Action action, Func<bool>? Enabled = null) : base(name)
        {
            this.action = action;
            this.enabled = Enabled;
        }

        public override async Task Execute()
        {
            await Task.Run(action);
        }

    }
}
