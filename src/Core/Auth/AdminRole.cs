namespace CompetitiveBackend.Core.Auth
{
    public class AdminRole : Role
    {
        public override bool IsAdmin() => true;
        public override bool IsPlayer() => false;
        public override string ToString() => "Admin";
    }

}
