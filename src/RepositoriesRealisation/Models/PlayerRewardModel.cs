using RepositoriesRealisation.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.Models
{
    public class PlayerRewardModel
    {
        public RewardDescriptionModel RewardDescription { get; set; }
        public AccountModel Player { get; set; }
        public CompetitionModel? Competition { get; set; }
        [ForeignKey("player_id"), Column("player_id")]
        public int PlayerID;
        [ForeignKey("competition_id"), Column("competition_id")]
        public int? CompetitionID;
        [ForeignKey("reward_description_id"), Column("reward_description_id")]
        public int RewardDescriptionID;
        [Column("creation_date")]
        public TimeSpan CreationDate;
    }
}
