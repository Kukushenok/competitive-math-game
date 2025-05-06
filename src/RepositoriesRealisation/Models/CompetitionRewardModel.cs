using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;
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
    public enum SupportedConditionType { 
        Rank,
        Place
    };

    [Table("competition_reward")]
    public class CompetitionRewardModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(CompetitionId))]
        public virtual CompetitionModel Competition { get; set; }
        [ForeignKey(nameof(RewardDescriptionId))]
        public virtual RewardDescriptionModel RewardDescription { get; set; }

        [Column("competition_id")]
        public int CompetitionId { get; set; }
        [Column("reward_description_id")]
        public int RewardDescriptionId { get; set; }
        [Column("condition_type")]
        public SupportedConditionType ConditionType { get; set; }
        [Column("min_place")]
        public int? MinPlace { get; set; }
        [Column("max_place")]
        public int? MaxPlace { get; set; }
        [Column("min_rank")]
        public float? MinRank { get; set; }
        [Column("max_rank")]
        public float? MaxRank { get; set; }
        public CompetitionRewardModel(int RewardDescriptionID, int CompetitionID, GrantCondition cnd)
        {
            this.RewardDescriptionId = RewardDescriptionID;
            this.CompetitionId = CompetitionID;
            SetCondition(cnd);
        }
        public CompetitionRewardModel()
        {

        }
        public void SetCondition(GrantCondition cnd)
        {
            MinPlace = null;
            MaxPlace = null;
            MinRank = null;
            MaxRank = null;
            if (cnd is RankGrantCondition ranked)
            {
                ConditionType = SupportedConditionType.Rank;
                MinRank = ranked.minRank;
                MaxRank = ranked.maxRank;
            }
            else if (cnd is PlaceGrantCondition placed)
            {
                ConditionType = SupportedConditionType.Place;
                MinPlace = placed.minPlace;
                MaxPlace = placed.maxPlace;
            }
            else
            {
                throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("Unspecified Grant Condition");
            }
        }
        public GrantCondition GetCondition()
        {
            switch (ConditionType)
            {
                case SupportedConditionType.Place:
                    return new PlaceGrantCondition(MinPlace!.Value, MaxPlace!.Value);
                case SupportedConditionType.Rank:
                    return new RankGrantCondition(MinRank!.Value, MaxRank!.Value);
                default:
                    throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("Unspecified Grant Condition");
            }
        }
        public CompetitionReward ToCoreModel()
        {
            GrantCondition cond = GetCondition();
            return new CompetitionReward(RewardDescriptionId, CompetitionId, RewardDescription.Name, RewardDescription.Description ?? "", cond, Id);
        }
    }
}
