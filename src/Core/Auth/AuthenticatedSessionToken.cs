namespace CompetitiveBackend.Core.Auth
{
    public class AuthenticatedSessionToken : SessionToken
    {
        private int Identifier;
        public AuthenticatedSessionToken(Role role, int id): base(role) { Identifier = id; }
        protected sealed override int GetAccountIdentifier() => Identifier;
        public sealed override bool IsAuthenticated() => true;
    }
}
