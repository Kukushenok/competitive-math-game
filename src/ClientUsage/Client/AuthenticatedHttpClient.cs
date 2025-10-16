
namespace ClientUsage.Client
{
    internal class AuthenticatedHttpClient : IHttpClient
    {
        private readonly IHttpClient _inner;
        private readonly string _token;

        public AuthenticatedHttpClient(IHttpClient inner, string token)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _token = token ?? throw new ArgumentNullException(nameof(token));
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            request.Headers.Add("Bearer", _token);
            return _inner.SendAsync(request, cancellationToken);
        }
    }
}