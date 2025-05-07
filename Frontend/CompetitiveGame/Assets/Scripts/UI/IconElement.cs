using CompetitiveBackend.BackendUsage.Exceptions;
using CompetitiveBackend.BackendUsage.Objects;
using CompetitiveBackend.BackendUsage.UseCases;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VContainer;

public class IconElement : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private Texture2D noTexture;
    private ISelfUseCase selfUseCase;
    private Texture2D currentTexture;
    [Inject]
    private void Construct(ISelfUseCase selfUseCase)
    {
        this.selfUseCase = selfUseCase;
        currentTexture = new Texture2D(2, 2);
        StartCoroutine(Refresh().ToCoroutine());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async UniTask Refresh()
    {
        image.texture = noTexture;
        try
        {
            LargeDataDTO dto = await selfUseCase.GetMyImage();
            if (dto.Data != null)
            {
                currentTexture.LoadImage(dto.Data);
                image.texture = currentTexture;
            }
        }
        catch(Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public void Click() => StartCoroutine(Refresh().ToCoroutine());
    public void UploadImage()
    {
        string[] dats = SFB.StandaloneFileBrowser.OpenFilePanel("Get ITEMS", "", "*.*", false);
        if (dats.Length > 0)
        {
            StartCoroutine(UploadFileRoutine(dats[0]).ToCoroutine());
        }
    }
    private async UniTask UploadFileRoutine(string file)
    {
        UnityWebRequest request = new UnityWebRequest(file);
        request.downloadHandler = new DownloadHandlerBuffer();
        await request.SendWebRequest().ToUniTask();
        LargeDataDTO dto = new LargeDataDTO(request.downloadHandler.data);
        await selfUseCase.UpdateMyImage(dto);
        await Refresh();
    }
    private void OnDestroy()
    {
        if(currentTexture != null) Destroy(currentTexture);
    }
}
