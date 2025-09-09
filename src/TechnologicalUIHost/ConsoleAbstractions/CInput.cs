using CompetitiveBackend.BackendUsage.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnologicalUIHost.ConsoleAbstractions
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
}
