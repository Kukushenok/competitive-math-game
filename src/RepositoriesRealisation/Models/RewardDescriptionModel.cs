using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompetitiveBackend.Core.Objects;

namespace RepositoriesRealisation.Models
{
    [Table("reward_description")]
    public class RewardDescriptionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("reward_name", TypeName = "varchar(64)")]
        public string Name { get; set; }
        [Column("description", TypeName = "varchar(64)")]
        public string? Description { get; set; }
        public RewardDescriptionModelIconImage IconImage { get; set; } = null!;
        public RewardDescriptionModel()
        {
            Name = string.Empty;
            Description = string.Empty;
        }

        public RewardDescriptionModel(string name, string? description = null)
        {
            Name = name;
            Description = description;
        }

        public RewardDescriptionModel(int id, string name, string? description = null)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public RewardDescription ToCoreRewardDescription()
        {
            return new RewardDescription(Name, Description ?? string.Empty, Id);
        }
    }

    [Table("reward_description")]
    public class RewardDescriptionModelIconImage : OneToOneEntity<RewardDescriptionModel>
    {
        [Column("icon_image", TypeName = "bytea")]
        public byte[]? IconImage { get; set; }
    }
}
