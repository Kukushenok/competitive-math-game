namespace CompetitiveBackend.Core.Auth
{
    public class GuestRole : Role
    {
        public override bool IsPlayer()
        {
            return false;
        }

        public override bool IsAdmin()
        {
            return false;
        }

        public override string ToString()
        {
            return "Guest";
        }
    }
}
