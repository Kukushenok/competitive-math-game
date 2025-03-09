using CompetitiveBackend.Core.Objects;

namespace CompetitiveBackend.LogicComponents
{
    public abstract class Role
    {
        public abstract bool IsPlayer();
        public abstract bool IsAdmin();
        public abstract override string ToString();
    }
    public class GuestRole: Role
    {
        public override bool IsPlayer() => false;
        public override bool IsAdmin() => false;
        public override string ToString() => "Guest";
    }
    public class PlayerRole: Role
    {
        public override bool IsAdmin() => false;
        public override bool IsPlayer() => true;
        public override string ToString() => "Player";
    }
    public class AdminRole: Role
    {
        public override bool IsAdmin() => true;
        public override bool IsPlayer() => false;
        public override string ToString() => "Admin";
    }
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
        protected virtual int GetAccountIdentifier() => 0;
    }
    public class UnauthenticatedSessionToken: SessionToken
    {
        public UnauthenticatedSessionToken() : base(new GuestRole()) { }

        public override bool IsAuthenticated() => false;
    }
    public class AuthenticatedSessionToken : SessionToken
    {
        private int Identifier;
        public AuthenticatedSessionToken(Role role, int id): base(role) { Identifier = id; }
        protected sealed override int GetAccountIdentifier() => Identifier;
        public sealed override bool IsAuthenticated() => true;
    }
    /// <summary>
    /// TODO: Это всё НА СТОРОНЕ БД. :P
    /// </summary>
    //public interface ISessionTokenCreator
    //{
    //    public SessionToken Create(in Account? account);
    //}
    //public class SessionTokenCreator : ISessionTokenCreator
    //{
    //    public SessionToken Create(in Account? account)
    //    {
    //        if (account == null || account.Id == null) return new GuestSessionToken();
    //        if (account.PrivilegyLevel > 0) return new AdminSessionToken((int)account.Id);
    //        return new PlayerSessionToken((int)account.Id);
    //    }
    //}
    
}
