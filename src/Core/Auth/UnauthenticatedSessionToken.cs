namespace CompetitiveBackend.Core.Auth
{
    public class UnauthenticatedSessionToken : SessionToken
    {
        public UnauthenticatedSessionToken()
            : base(new GuestRole())
        {
        }

        public override bool IsAuthenticated()
        {
            return false;
        }
    }
}
