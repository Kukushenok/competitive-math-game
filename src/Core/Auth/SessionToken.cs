namespace CompetitiveBackend.Core.Auth
{
    public abstract class SessionToken
    {
        public readonly Role Role;
        public SessionToken(Role role)
        {
            Role = role;
        }

        public abstract bool IsAuthenticated();
        public bool TryGetAccountIdentifier(out int identifier)
        {
            identifier = 0;
            if (IsAuthenticated())
            {
                identifier = GetAccountIdentifier();
                return true;
            }

            return false;
        }

        protected virtual int GetAccountIdentifier()
        {
            return 0;
        }
    }
}
