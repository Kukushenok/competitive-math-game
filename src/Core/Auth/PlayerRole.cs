namespace CompetitiveBackend.Core.Auth
{
    public class PlayerRole : Role
    {
        public override bool IsAdmin()
        {
            return false;
        }

        public override bool IsPlayer()
        {
            return true;
        }

        public override string ToString()
        {
            return "Player";
        }
    }
}
