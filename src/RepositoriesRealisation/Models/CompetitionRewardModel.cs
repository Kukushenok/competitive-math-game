using CompetitiveBackend.Core.Objects;
using Core.RewardCondition;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        public CompetitionModel Competition { get; }
        public RewardDescriptionModel RewardDescription { get; }

        [ForeignKey("competition_id"), Column("competition_id")]
        public int CompetitionId { get; set; }
        [ForeignKey("reward_description_id"), Column("reward_description_id")]
        public int RewardDescriptionId { get; set; }
        [Column("condition")]
        public JsonDocument Condition { get; set; }
        public CompetitionRewardModel(int RewardDescriptionID, int CompetitionID, JsonDocument condition)
        {
            this.RewardDescriptionId = RewardDescriptionId;
            this.Condition = condition;
            this.CompetitionId = CompetitionID;
        }
        public CompetitionRewardModel()
        {

        }
        public CompetitionReward ToCoreModel()
        {
            if (!GrantConditionConverter.FromJSON(Condition, out GrantCondition cond)) return null!;
            return new CompetitionReward(RewardDescriptionId, CompetitionId, RewardDescription.Name, RewardDescription.Description ?? "", cond, Id);
        }
    }
}
