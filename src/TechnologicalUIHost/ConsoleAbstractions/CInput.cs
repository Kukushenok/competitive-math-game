namespace TechnologicalUIHost.ConsoleAbstractions
{
    public interface IConsoleInput
    {
        string PromtInput(string promt = "> ");
        byte[] ReadData(string promt = "> ");
    }

    public interface IConsoleOutput
    {
        void PromtOutput(string promt);
        void SaveData(byte[] largeData, string promt = "");
    }

    public interface IConsole : IConsoleInput, IConsoleOutput
    {
    }
}
