using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.Models
{
    [Table("competition_reward")]
    public class CompetitionRewardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey("reward_description_id"), Column("reward_description_id")]
        public int RewardDescriptionId { get; set; }
        [Column("condition_name")]
        public string Condition { get; set; }
        [Column("condition_description")]
        public string ConditionDescription { get; set; }
        public CompetitionRewardModel(int RewardDescriptionID, string Condition, string ConditionDescription)
        {
            this.RewardDescriptionId = RewardDescriptionId;
            this.Condition = Condition;
            this.ConditionDescription = ConditionDescription;
        }
        public CompetitionRewardModel()
        {
            Condition = "";
            ConditionDescription = "";
        }
    }
}
