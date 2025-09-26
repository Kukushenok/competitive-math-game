using BackendUsage.OpenAPI;
using UnityEngine;

public class PlayerPrefsAuthCache : IAuthCache
{
    public string AuthToken { get => PlayerPrefs.GetString(nameof(AuthToken)); set => PlayerPrefs.SetString(nameof(AuthToken), value); }
}
