using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompetitiveBackend.Core.Objects;
using CompetitiveBackend.Core.RewardCondition;

namespace RepositoriesRealisation.Models
{
    public enum SupportedConditionType
    {
        Rank,
        Place,
    }

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
        public CompetitionRewardModel(int rewardDescriptionID, int competitionID, GrantCondition cnd)
        {
            RewardDescriptionId = rewardDescriptionID;
            CompetitionId = competitionID;
            Competition = null!;
            RewardDescription = null!;
            SetCondition(cnd);
        }

        public CompetitionRewardModel()
        {
            RewardDescriptionId = 0;
            CompetitionId = 0;
            Competition = null!;
            RewardDescription = null!;
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
                MinRank = ranked.MinRank;
                MaxRank = ranked.MaxRank;
            }
            else if (cnd is PlaceGrantCondition placed)
            {
                ConditionType = SupportedConditionType.Place;
                MinPlace = placed.MinPlace;
                MaxPlace = placed.MaxPlace;
            }
            else
            {
                throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("Unspecified Grant Condition");
            }
        }

        public GrantCondition GetCondition()
        {
            return ConditionType switch
            {
                SupportedConditionType.Place => new PlaceGrantCondition(MinPlace!.Value, MaxPlace!.Value),
                SupportedConditionType.Rank => new RankGrantCondition(MinRank!.Value, MaxRank!.Value),
                _ => throw new CompetitiveBackend.Repositories.Exceptions.IncorrectOperationException("Unspecified Grant Condition"),
            };
        }

        public CompetitionReward ToCoreModel()
        {
            GrantCondition cond = GetCondition();
            return new CompetitionReward(RewardDescriptionId, CompetitionId, RewardDescription.Name, RewardDescription.Description ?? string.Empty, cond, Id);
        }
    }
}
