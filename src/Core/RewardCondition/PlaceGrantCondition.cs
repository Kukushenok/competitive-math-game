namespace CompetitiveBackend.Core.RewardCondition
{
    public class PlaceGrantCondition : GrantCondition
    {
        public override string Type => "place";
        public int MinPlace;
        public int MaxPlace;
        public PlaceGrantCondition(int minPlace, int maxPlace)
        {
            this.MinPlace = minPlace;
            this.MaxPlace = maxPlace;
        }
    }
}
