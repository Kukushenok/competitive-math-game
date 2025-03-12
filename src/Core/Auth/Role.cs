namespace CompetitiveBackend.Core.Auth
{
    public abstract class Role
    {
        public abstract bool IsPlayer();
        public abstract bool IsAdmin();
        public abstract override string ToString();
    }
}
