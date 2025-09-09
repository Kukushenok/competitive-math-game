using CompetitiveFrontend.OpenAPIClient.Client;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BackendUsage.OpenAPI
{
    public class GlobalConfigurationSpoofer: IReadableConfiguration
    {
        private IReadableConfiguration configuration;
        private IAuthCache authCache;
        public GlobalConfigurationSpoofer(IReadableConfiguration configuration, IAuthCache authCache)
        {
            this.configuration = configuration;
            this.authCache = authCache;
        }

        public string AccessToken => configuration.AccessToken;

        public IDictionary<string, string> ApiKey => configuration.ApiKey;

        public IDictionary<string, string> ApiKeyPrefix => configuration.ApiKeyPrefix;

        public string BasePath => configuration.BasePath;

        public string DateTimeFormat => configuration.DateTimeFormat;

        public IDictionary<string, string> DefaultHeader => configuration.DefaultHeader;

        public IDictionary<string, string> DefaultHeaders => configuration.DefaultHeaders;

        public string TempFolderPath => configuration.TempFolderPath;

        public TimeSpan Timeout => configuration.Timeout;

        public WebProxy Proxy => configuration.Proxy;

        public string UserAgent => configuration.UserAgent;

        public string Username => configuration.Username;

        public string Password => configuration.Password;

        public bool UseDefaultCredentials => configuration.UseDefaultCredentials;

        public IReadOnlyDictionary<string, List<IReadOnlyDictionary<string, object>>> OperationServers => configuration.OperationServers;

        public X509CertificateCollection ClientCertificates => configuration.ClientCertificates;

        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback => configuration.RemoteCertificateValidationCallback;

        public string GetApiKeyWithPrefix(string apiKeyIdentifier)
        {
            return authCache.AuthToken;
        }

        public string GetOperationServerUrl(string operation, int index)
        {
            return configuration.GetOperationServerUrl(operation, index);
        }
    }
}
