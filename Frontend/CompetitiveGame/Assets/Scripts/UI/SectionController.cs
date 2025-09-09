using Cysharp.Threading.Tasks;
using UnityEngine;

public class SectionController: MonoBehaviour, ISectionController
{
    [field: SerializeField] public string Name { get; set; }
    public bool Shown { get; private set; }

    public UniTask Close()
    {
        Shown = false;
        gameObject.SetActive(false);
        return UniTask.CompletedTask;
    }

    public UniTask Show()
    {
        Shown = true;
        gameObject.SetActive(true);
        return UniTask.CompletedTask;
    }
}
