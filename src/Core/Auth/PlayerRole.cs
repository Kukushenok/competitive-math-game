namespace CompetitiveBackend.Core.Auth
{
    public class PlayerRole: Role
    {
        public override bool IsAdmin() => false;
        public override bool IsPlayer() => true;
        public override string ToString() => "Player";
    }
}
