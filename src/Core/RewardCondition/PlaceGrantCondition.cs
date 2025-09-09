using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveBackend.Core.RewardCondition
{
    public class PlaceGrantCondition : GrantCondition
    {
        public override string Type => "place";
        public int minPlace;
        public int maxPlace;
        public PlaceGrantCondition(int minPlace, int maxPlace)
        {
            this.minPlace = minPlace;
            this.maxPlace = maxPlace;
        }
    }
}
