using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RepositoriesRealisation.Models
{
    [Table("competition")]
    public class CompetitionModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("competition_name", TypeName = "varchar(64)")]
        public string Name { get; set; }
        [Column("description", TypeName = "varchar(128)")]
        public string? Description { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }
        [Column("end_time")]
        public DateTime EndTime { get; set; }
        [Column("has_ended")]
        public bool HasEnded { get; set; } = false;
        public virtual RiddleGameSettingsModel RiddleGameSettings { get; set; }
        public CompetitionModel(int id, string name, string? description, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            RiddleGameSettings = new RiddleGameSettingsModel(id, new CompetitiveBackend.Core.Objects.Riddles.RiddleGameSettings(0, 0, 0, null, 0));
        }

        public CompetitionModel(string name, string? description, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            RiddleGameSettings = null!;
        }

        public CompetitionModel()
        {
            Name = string.Empty;
            Description = string.Empty;
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            RiddleGameSettings = null!;
        }

        public Competition ToCoreModel()
        {
            return new Competition(Name, Description ?? string.Empty, StartTime, EndTime, Id);
        }
    }

    [Obsolete("Defunct")]
    [Table("competition_level")]
    public class CompetitionLevelDataModel
    {
        public CompetitionModel Competition { get; set; } = null!;
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey("competition_id")]
        [Column("competition_id", TypeName = "int")]
        public int CompetitionID { get; set; }
        [Column("platform", TypeName = "varchar(32)")]
        public string Platform { get; set; }
        [Column("version_key", TypeName = "int")]
        public int VersionKey { get; set; }
        public CompetitionLevelDataModelData LevelData { get; set; } = null!;
        public CompetitionLevelDataModel()
        {
            CompetitionID = 0;
            Platform = string.Empty;
            VersionKey = 0;
        }

        public CompetitionLevelDataModel(LevelDataInfo info)
        {
            CompetitionID = info.CompetitionID;
            Platform = info.PlatformName;
            VersionKey = info.VersionCode;
        }
    }

    [Obsolete("Defunct")]
    [Table("competition_level")]
    public class CompetitionLevelDataModelData : OneToOneEntity<CompetitionLevelDataModel>
    {
        [Column("level_data", TypeName = "bytea")]
        public byte[]? LevelData { get; set; }
    }
}
