using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
namespace CompetitiveFrontend.OpenAPIClient.Client
{
    [CreateAssetMenu(fileName = "ConfigurationScriptableObject", menuName = "Scriptable Objects/ConfigurationScriptableObject")]
    public class ConfigurationScriptableObject : ScriptableObject, IReadableConfiguration
    {
        [SerializeField] private string serverURL;
        [SerializeField] private double timeoutSeconds = 100;
        public string AccessToken => DEFAULT.AccessToken;

        public IDictionary<string, string> ApiKey => DEFAULT.ApiKey;

        public IDictionary<string, string> ApiKeyPrefix => DEFAULT.ApiKeyPrefix;

        public string BasePath => serverURL;

        public string DateTimeFormat => DEFAULT.DateTimeFormat;

        [Obsolete]
        public IDictionary<string, string> DefaultHeader => DEFAULT.DefaultHeader;

        public IDictionary<string, string> DefaultHeaders => DEFAULT.DefaultHeaders;

        public string TempFolderPath => DEFAULT.TempFolderPath;

        public TimeSpan Timeout => TimeSpan.FromSeconds(timeoutSeconds);

        public WebProxy Proxy => DEFAULT.Proxy;

        public string UserAgent => DEFAULT.UserAgent;

        public string Username => DEFAULT.Username;

        public string Password => DEFAULT.Password;

        public bool UseDefaultCredentials => DEFAULT.UseDefaultCredentials;

        public IReadOnlyDictionary<string, List<IReadOnlyDictionary<string, object>>> OperationServers => DEFAULT.OperationServers;

        public X509CertificateCollection ClientCertificates => DEFAULT.ClientCertificates;

        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback => DEFAULT.RemoteCertificateValidationCallback;

        private IReadableConfiguration DEFAULT => GlobalConfiguration.Instance;

        public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        {
            return DEFAULT.GetApiKeyWithPrefix(apiKeyIdentifier);
        }

        public string GetOperationServerUrl(string operation, int index)
        {
            return DEFAULT.GetOperationServerUrl(operation, index);
        }

    }
}