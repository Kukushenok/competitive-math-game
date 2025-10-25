namespace CompetitiveBackend.Core.Auth
{
    public class AdminRole : Role
    {
        public override bool IsAdmin()
        {
            return true;
        }

        public override bool IsPlayer()
        {
            return false;
        }

        public override string ToString()
        {
            return "Admin";
        }
    }
}
