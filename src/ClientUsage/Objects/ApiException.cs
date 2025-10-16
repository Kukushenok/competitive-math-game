// File: IHttpClient.cs
namespace ClientUsage.Objects
{
    public class ApiException : Exception
    {
        public int Status { get; }
        public string? Title { get; }
        public string? Detail { get; }

        public ApiException(int status, string? title = null, string? detail = null)
            : base($"API error {status}: {title ?? "(no title)"} - {detail ?? "(no detail)"}")
        {
            Status = status;
            Title = title;
            Detail = detail;
        }
    }
}
