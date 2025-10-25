// File: IHttpClient.cs

// File: IHttpClient.cs
namespace ClientUsage.Client
{
    public class HttpClientWrapper : IHttpClient, IDisposable
    {
        private readonly HttpClient httpClient;
        private bool disposed;

        public HttpClientWrapper(HttpClient httpClient)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            return httpClient.SendAsync(request, cancellationToken);
        }

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            httpClient.Dispose();
            disposed = true;
        }
    }
}
