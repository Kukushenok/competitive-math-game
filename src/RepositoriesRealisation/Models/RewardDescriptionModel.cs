using CompetitiveBackend.Core.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        [Column("icon_image", TypeName = "bytea")]
        public byte[]? IconImage { get; set; }
        [Column("ingame_data", TypeName ="bytea")]
        public byte[]? InGameData { get; set; }
        public RewardDescriptionModel()
        {

        }
        public RewardDescriptionModel(string name, string? description = null, byte[]? iconImage = null, byte[]? inGameData = null)
        {
            Name = name;
            Description = description;
            IconImage = iconImage;
            InGameData = inGameData;
        }
        public RewardDescriptionModel(int id, string name, string? description = null, byte[]? iconImage = null, byte[]? inGameData = null)
        {
            Id = id;
            Name = name;
            Description = description;
            IconImage = iconImage;
            InGameData = inGameData;
        }
        public RewardDescription ToCoreRewardDescription()
        {
            return new RewardDescription(Name, Description ?? "", Id);
        }
    }
}
