using TMPro;
using UnityEngine;
using VContainer;

public class WindowButtonView: MonoBehaviour
{
    private IWindowManager windowManager;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;
    [Inject]
    private void Construct(IWindowManager windowManager)
    {
        this.windowManager = windowManager;
    }
    private void Awake()
    {
        foreach (string key in windowManager.Sections)
        {
            GameObject gm = Instantiate(buttonPrefab, buttonParent);
            gm.SetActive(true);
            gm.GetComponentInChildren<TextMeshProUGUI>().text = key;
            string s = key;
            gm.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => ChangeSection(s));
        }
    }
    private void ChangeSection(string key)
    {
        windowManager.ChangeSection(key);
    }

}
