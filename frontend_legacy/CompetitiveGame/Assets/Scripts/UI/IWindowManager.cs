using System.Collections.Generic;

public interface IWindowManager
{
    public IList<string> Sections { get; }
    public bool ChangeSection(string section);
}
