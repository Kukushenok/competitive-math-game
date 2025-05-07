public interface IErrorMessagesConfiguration
{
    public string ServerErrorMessage { get; }
    public string NotPermittedMessage { get; }
    public string NotAuthorizedMessage { get; }
    public string BadRequestMessage { get; }
}
