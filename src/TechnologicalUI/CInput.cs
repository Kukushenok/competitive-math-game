using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalUI
{
    public interface IConsoleInput
    {
        public string PromtInput(string promt = "> ");
        public byte[] ReadData(string promt = "> ");
    }
    public interface IConsoleOutput
    {
        public void PromtOutput(string promt);
        public void SaveData(byte[] largeData, string promt = "");
    }
    public interface IConsole : IConsoleInput, IConsoleOutput { }
    public class ConsoleInOut : IConsole
    { 
        public string PromtInput(string promt = "> ")
        {
            Console.Write(promt);
            return Console.ReadLine()!;
        }

        public void PromtOutput(string promt)
        {
            Console.WriteLine(promt);
        }

        public byte[] ReadData(string promt = "> ")
        {
            Console.Write(promt);
            Console.Write("[Введите путь к файлу] ");
            string path = Console.ReadLine()!;
            try
            {
                byte[] data = File.ReadAllBytes(path);
                return data;
            }
            catch(Exception ex)
            {
                throw new FormatException(ex.Message);
            }
        }

        public void SaveData(byte[] largeData, string promt = "")
        {
            Console.Write(promt);
            Console.Write("[Введите путь к файлу] ");
            string path = Console.ReadLine()!;
            try
            {
                File.WriteAllBytes(path, largeData);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
            }
        }
    }
}
