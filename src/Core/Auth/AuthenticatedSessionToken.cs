namespace CompetitiveBackend.Core.Auth
{
    public class AuthenticatedSessionToken : SessionToken
    {
        private readonly int identifier;
        public AuthenticatedSessionToken(Role role, int id)
            : base(role)
        {
            identifier = id;
        }

        protected sealed override int GetAccountIdentifier()
        {
            return identifier;
        }

        public sealed override bool IsAuthenticated()
        {
            return true;
        }
    }
}
