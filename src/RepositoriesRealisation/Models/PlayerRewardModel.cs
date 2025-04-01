using RepositoriesRealisation.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.Models
{
    [Table("player_reward")]
    public class PlayerRewardModel
    {
        [ForeignKey(nameof(RewardDescriptionID))]
        public RewardDescriptionModel RewardDescription { get; set; } = null!;
        [ForeignKey(nameof(PlayerID))]
        public AccountModel Player { get; set; } = null!;
        [ForeignKey(nameof(CompetitionID))]
        public CompetitionModel? Competition { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("player_id")]
        public int PlayerID { get; set; }
        [ForeignKey("competition_id"), Column("competition_id")]
        public int? CompetitionID { get; set; }
        [ForeignKey("reward_description_id"), Column("reward_description_id")]
        public int RewardDescriptionID { get; set; }
        [Column("creation_date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public TimeSpan CreationDate { get; set; }
        public PlayerRewardModel(int playerID, int RewardDescription, int? CompetitionID = null)
        {
            PlayerID = playerID;
            RewardDescriptionID = RewardDescription;
            this.CompetitionID = CompetitionID;
        }
    }
}
