using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;
using System.Windows.Input;
namespace TechnologicalUI.Command
{
    public abstract class CompositeCommandBlock: NamedConsoleCommand
    {
        private class JoinedCommandBlock : CompositeCommandBlock
        {
            private List<IConsoleMenuCommand> Commands = new List<IConsoleMenuCommand>();
            public JoinedCommandBlock(string name, params CompositeCommandBlock[] commands): base(name)
            {
                foreach(var cmdComplex in commands)
                {
                    foreach(var cmd in cmdComplex.GetCommands())
                    {
                        Commands.Add(cmd);
                    }
                }
            }
            public JoinedCommandBlock(string name, params IConsoleMenuCommand[] menuCommands): base(name)
            {
                foreach(var cmd in menuCommands)
                {
                    Commands.Add(cmd);
                }
            }
            protected override IEnumerable<IConsoleMenuCommand> GetCommands() => Commands;
        }
        protected CompositeCommandBlock(string name) : base(name)
        {
        }

        public sealed override async Task Execute(IConsole console)
        {
            while (true)
            {

                List<IConsoleMenuCommand> commands = GetCommands().ToList();
                console.PromtOutput("Введите номер команды: ");
                console.PromtOutput("0 : Выход");
                int idx = 1;
                foreach (IConsoleMenuCommand cmd in commands)
                {
                    if(cmd.Enabled)
                        console.PromtOutput($"{idx} : {cmd.GetLabel()}");
                    idx++;
                }
                try
                {
                    int commandIdx = console.ReadInt("> ");
                    commandIdx--;
                    if (commandIdx < 0) break;
                    foreach(IConsoleMenuCommand cmd in commands)
                    {
                        if (cmd.Enabled && commandIdx == 0)
                        {
                            await cmd.Execute(console);
                            break;
                        }
                        commandIdx--;
                    }
                    if(commandIdx != 0)
                    {
                        Console.WriteLine("Команда не найдена");
                    }
                }
                catch(FormatException)
                {
                    Console.WriteLine("Неверный ввод данных");
                }
                catch (RepositoryException ex)
                {
                    Console.WriteLine($"Возникла ошибка на уровне репозитория: {ex.Message}");
                }
                catch (ServiceException ex)
                {
                    Console.WriteLine($"Возникла ошибка на уровне сервисов: {ex.Message}");
                }
                catch(UseCaseException ex)
                {
                    Console.WriteLine($"Возникла ошибка на уровне пользовательских сценариев: {ex.Message}");
                }
            }
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
            return new JoinedCommandBlock(cmd.Name, cmd.GetCommands().Concat(additionalCommands).ToArray());
        }
    }
}
