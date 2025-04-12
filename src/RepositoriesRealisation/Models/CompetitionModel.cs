using CompetitiveBackend.Core.Objects;
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

        public CompetitionModelLevelData LevelData { get; set; } = null!;
        public CompetitionModel(int id, string name, string? description, DateTime startTime, DateTime endTime)
        {
            Id = id;
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
        }
        public CompetitionModel(string name, string? description, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
        }
        public CompetitionModel()
        {
            Name = "";
        }
        public Competition ToCoreModel() => new Competition(Name, Description ?? "", StartTime, EndTime, Id);
    }
    [Table("competition")]
    public class CompetitionModelLevelData: OneToOneEntity<CompetitionModel>
    {
        [Column("level_data", TypeName = "bytea")]
        public byte[]? LevelData { get; set; }
    }
}
