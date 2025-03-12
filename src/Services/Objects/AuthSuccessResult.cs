namespace CompetitiveBackend.Services.Objects
{
    public record AuthSuccessResult(string Token, string RoleName, int AccountID)
    {
    }
}
