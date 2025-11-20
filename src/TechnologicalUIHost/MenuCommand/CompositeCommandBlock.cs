using CompetitiveBackend.BackendUsage.Exceptions;
using TechnologicalUIHost.ConsoleAbstractions;
namespace TechnologicalUIHost.Command
{
    public abstract class CompositeCommandBlock : NamedConsoleCommand
    {
        private sealed class JoinedCommandBlock : CompositeCommandBlock
        {
            private readonly List<IConsoleMenuCommand> commands = [];
            public JoinedCommandBlock(string name, params CompositeCommandBlock[] commands)
                : base(name)
            {
                foreach (CompositeCommandBlock cmdComplex in commands)
                {
                    foreach (IConsoleMenuCommand cmd in cmdComplex.GetCommands())
                    {
                        this.commands.Add(cmd);
                    }
                }
            }

            public JoinedCommandBlock(string name, params IConsoleMenuCommand[] menuCommands)
                : base(name)
            {
                foreach (IConsoleMenuCommand cmd in menuCommands)
                {
                    commands.Add(cmd);
                }
            }

            protected override IEnumerable<IConsoleMenuCommand> GetCommands()
            {
                return commands;
            }
        }

        protected CompositeCommandBlock(string name)
            : base(name)
        {
        }

        public sealed override async Task Execute(IConsole console)
        {
            while (true)
            {
                var commands = GetCommands().ToList();
                console.PromtOutput("Введите номер команды: ");
                console.PromtOutput("0 : Выход");
                int idx = 1;
                foreach (IConsoleMenuCommand cmd in commands)
                {
                    if (cmd.Enabled)
                    {
                        console.PromtOutput($"{idx} : {cmd.GetLabel()}");
                    }

                    idx++;
                }

                if (!await AskAndExec(console, commands))
                {
                    break;
                }
            }
        }

        private static async Task<bool> AskAndExec(IConsole console, List<IConsoleMenuCommand> commands)
        {
            try
            {
                int commandIdx = console.ReadInt("> ");
                commandIdx--;
                if (commandIdx < 0)
                {
                    return false;
                }

                foreach (IConsoleMenuCommand cmd in commands)
                {
                    if (cmd.Enabled && commandIdx == 0)
                    {
                        await cmd.Execute(console);
                        break;
                    }

                    commandIdx--;
                }

                if (commandIdx != 0)
                {
                    console.PromtOutput("Команда не найдена");
                }
            }
            catch (FormatException)
            {
                console.PromtOutput("Неверный ввод данных");
            }
            catch (UseCaseException ex)
            {
                console.PromtOutput($"Возникла ошибка на уровне пользовательских сценариев: {ex.Message}");
            }
            catch (Exception ex)
            {
                console.PromtOutput($"Возникла ошибка при выполнении пользовательского сценария: {ex.Message}");
            }

            return true;
        }

        protected abstract IEnumerable<IConsoleMenuCommand> GetCommands();
        public static CompositeCommandBlock Join(string name, params CompositeCommandBlock[] compositeCommandBlocks)
        {
            return new JoinedCommandBlock(name, compositeCommandBlocks);
        }

        public static CompositeCommandBlock Sum(string name, params IConsoleMenuCommand[] compositeCommandBlocks)
        {
            return new JoinedCommandBlock(name, compositeCommandBlocks);
        }

        public static CompositeCommandBlock Add(CompositeCommandBlock cmd, params IConsoleMenuCommand[] additionalCommands)
        {
            return new JoinedCommandBlock(cmd.name, cmd.GetCommands().Concat(additionalCommands).ToArray());
        }
    }
}
