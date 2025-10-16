// File: IHttpClient.cs

// File: IHttpClient.cs

using System.Net.Http.Json;

namespace ClientUsage.Client
{
    public class HttpClientWrapper : IHttpClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private bool _disposed;

        public HttpClientWrapper(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
            => _httpClient.SendAsync(request, cancellationToken);

        public void Dispose()
        {
            if (_disposed) return;
            _httpClient.Dispose();
            _disposed = true;
        }
    }
}
