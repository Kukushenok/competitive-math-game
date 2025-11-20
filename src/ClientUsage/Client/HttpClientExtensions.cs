// File: IHttpClient.cs
using System.Net.Http.Headers;
using System.Text;
using ClientUsage.Objects;
using Newtonsoft.Json;

// File: HttpClientExtensions.cs
namespace ClientUsage.Client
{
    public static class HttpClientExtensions
    {
        private static void AddHeadersTo(HttpRequestMessage req, IDictionary<string, string>? headers)
        {
            if (headers == null)
            {
                return;
            }

            foreach (KeyValuePair<string, string> kv in headers)
            {
                if (string.Equals(kv.Key, "Authorization", StringComparison.OrdinalIgnoreCase))
                {
                    string[]? parts = kv.Value?.Split(' ', 2);
                    if (parts?.Length == 2)
                    {
                        req.Headers.Authorization = new AuthenticationHeaderValue(parts[0], parts[1]);
                    }
                    else if (parts?.Length == 1)
                    {
                        req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", parts[0]);
                    }

                    continue;
                }

                if (!req.Headers.TryAddWithoutValidation(kv.Key, kv.Value))
                {
                    req.Content ??= new StringContent(string.Empty);

                    req.Content.Headers.TryAddWithoutValidation(kv.Key, kv.Value);
                }
            }
        }

        private static async Task HandleNonSuccess(HttpResponseMessage resp)
        {
            if (resp.IsSuccessStatusCode)
            {
                return;
            }

            string content = string.Empty;
            try
            {
                content = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
                ProblemDetails? pd = JsonConvert.DeserializeObject<ProblemDetails>(content);
                if (pd != null)
                {
                    throw new ApiException(pd.Status ?? (int)resp.StatusCode, pd.Title, pd.Detail);
                }
            }
            catch (JsonException)
            {
                // Fallthrough
            }

            // If we get here, throw generic
            throw new ApiException((int)resp.StatusCode, resp.ReasonPhrase, content);
        }

        private static async Task<T> ReadAsJsonAsync<T>(HttpResponseMessage resp)
        {
            string s = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(s)!;
        }

        public static async Task<T> Get<T>(this IHttpClient client, string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, url);
            AddHeadersTo(req, headers);
            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
            return await ReadAsJsonAsync<T>(resp).ConfigureAwait(false);
        }

        public static async Task<T> Post<T>(this IHttpClient client, string url, object? body = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
            return await ReadAsJsonAsync<T>(resp).ConfigureAwait(false);
        }

        public static async Task PostNoContent(this IHttpClient client, string url, object? body = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
        }

        public static async Task<T> Put<T>(this IHttpClient client, string url, object? body = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Put, url);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
            return await ReadAsJsonAsync<T>(resp).ConfigureAwait(false);
        }

        public static async Task PatchNoContent(this IHttpClient client, string url, object? body = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(new HttpMethod("PATCH"), url);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
        }

        public static async Task DeleteNoContent(this IHttpClient client, string url, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, url);
            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
        }

        public static async Task PostMultipartNoContent(
            this IHttpClient client,
            string url,
            Stream fileStream,
            string fileFieldName,
            string fileName,
            string contentType,
            IDictionary<string, string>? additionalFields = null,
            IDictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, url);
            var multipart = new MultipartFormDataContent();

            var fileContent = new StreamContent(fileStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            multipart.Add(fileContent, fileFieldName, fileName);

            if (additionalFields != null)
            {
                foreach (KeyValuePair<string, string> kv in additionalFields)
                {
                    multipart.Add(new StringContent(kv.Value ?? string.Empty), kv.Key);
                }
            }

            req.Content = multipart;
            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
        }

        public static async Task PutNoContent(this IHttpClient client, string url, object? body = null, IDictionary<string, string>? headers = null, CancellationToken cancellationToken = default)
        {
            var req = new HttpRequestMessage(HttpMethod.Put, url);
            if (body != null)
            {
                string json = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            AddHeadersTo(req, headers);

            HttpResponseMessage resp = await client.SendAsync(req, cancellationToken).ConfigureAwait(false);
            await HandleNonSuccess(resp).ConfigureAwait(false);
        }
    }
}
