using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.Repositories.Exceptions;
using CompetitiveBackend.Services.Exceptions;

namespace TechnologicalUI.Command
{
    public abstract class CompositeCommandBlock: NamedConsoleCommand
    {
        protected CompositeCommandBlock(string name) : base(name)
        {
        }

        public sealed override async Task Execute()
        {
            while (true)
            {

                List<IConsoleMenuCommand> commands = GetCommands().ToList();
                Console.WriteLine("Введите номер команды: ");
                Console.WriteLine("0 : Выход");
                int idx = 1;
                foreach (IConsoleMenuCommand cmd in commands)
                {
                    if(cmd.Enabled)
                        Console.WriteLine($"{idx} : {cmd.GetLabel()}");
                    idx++;
                }
                try
                {
                    int commandIdx = CInput.ReadInt("> ");
                    commandIdx--;
                    if (commandIdx < 0) break;
                    foreach(IConsoleMenuCommand cmd in commands)
                    {
                        if (cmd.Enabled && commandIdx == 0)
                        {
                            await cmd.Execute();
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
    }
}
