using CompetitiveFrontend.OpenAPIClient.Api;
using CompetitiveFrontend.OpenAPIClient.Client;
using VContainer;
using VContainer.Unity;
using CompetitiveBackend.BackendUsage.Objects;
using BackendUsage.OpenAPI;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
public class SampleLifetimeScope : LifetimeScope
{
    [SerializeField] private ConfigurationScriptableObject configurationScriptableObject;
    [SerializeField] private List<GameObject> sectionObjects;
    [SerializeField] private Transform mainWindow;
    [SerializeField] private GameObject coolMessageLogger;
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<IAuthCache, PlayerPrefsAuthCache>(Lifetime.Singleton);
        builder.Register<IWindowManager, WindowManager>(Lifetime.Singleton);
        
        builder.InstallRemoteUseCases(configurationScriptableObject);
        List<ISectionController> ctrls = new List<ISectionController>();
        foreach(var gm in sectionObjects)
        {
            ctrls.Add(Instantiate(gm, mainWindow).GetComponent<ISectionController>());
        }
        builder.RegisterInstance<IReadOnlyCollection<ISectionController>>(ctrls);

    }
}
