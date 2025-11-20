namespace ClientUsage.Client
{
    internal sealed class AuthenticatedHttpClient : IHttpClient
    {
        private readonly IHttpClient inner;
        private readonly string token;

        public AuthenticatedHttpClient(IHttpClient inner, string token)
        {
            this.inner = inner ?? throw new ArgumentNullException(nameof(inner));
            this.token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            request.Headers.Add("Bearer", token);
            return inner.SendAsync(request, cancellationToken);
        }
    }
}