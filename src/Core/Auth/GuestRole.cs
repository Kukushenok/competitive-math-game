namespace CompetitiveBackend.Core.Auth
{
    public class GuestRole : Role
    {
        public override bool IsPlayer() => false;
        public override bool IsAdmin() => false;
        public override string ToString() => "Guest";
    }
}
