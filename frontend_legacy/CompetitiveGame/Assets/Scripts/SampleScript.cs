using BackendUsage.OpenAPI;
using CompetitiveBackend.BackendUsage.UseCases;
using CompetitiveFrontend.OpenAPIClient.Api;
using CompetitiveFrontend.OpenAPIClient.Client;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SampleScript : MonoBehaviour
{
    private IAuthUseCase authUseCase;
    private IAuthCache authCache;
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private TMP_InputField emailField;
    [Inject]
    private void Construct(IAuthUseCase authUseCase, IAuthCache authCache)
    {
        this.authUseCase = authUseCase;
        this.authCache = authCache;
    }
    public void RegisterButtonClick()
    {
        nameField.transform.DOKill(true);
        nameField.transform.DOShakeScale(0.5f, 0.05f, 20);
        StartCoroutine(DoRegister().ToCoroutine());
    }
    public async UniTask DoRegister()
    {
        var result = await authUseCase.Register(new CompetitiveBackend.BackendUsage.Objects.AccountCreationDTO(nameField.text, passwordField.text, emailField.text));
        authCache.AuthToken = result.Token;
    }
}
