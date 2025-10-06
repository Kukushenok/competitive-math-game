using Cysharp.Threading.Tasks;

public interface ISectionController
{
    public string Name { get; set; }
    public bool Shown { get; }
    public UniTask Show();
    public UniTask Close();
}
