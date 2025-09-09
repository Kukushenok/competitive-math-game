using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
public class WindowManager : IWindowManager
{

    private Dictionary<string, ISectionController> sections = new Dictionary<string, ISectionController>();
    public IList<string> Sections => sections.Keys.AsReadOnlyList();
    private bool changeLock = true;
    private ISectionController current = null;

    public bool ChangeSection(string section)
    {
        if (current.Name == section || !changeLock || !sections.ContainsKey(section))
        {
            return false;
        }
        changeLock = true;
        ChangeTo(sections[section]).Forget();
        return true;
    }
    private async UniTaskVoid ChangeTo(ISectionController controller)
    {
        await current.Close();
        await controller.Show();
        current = controller;
        changeLock = false;
    }
    public WindowManager(IReadOnlyCollection<ISectionController> secList)
    {
        foreach (var section in secList)
        {
            sections.Add(section.Name, section);
        }
        current = secList.First();
        current.Show().Forget();
    }
    
}
