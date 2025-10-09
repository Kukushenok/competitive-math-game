using CompetitiveBackend.Core.Objects.Riddles;
using RepositoriesRealisation.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CompetitiveBackend.Repositories
{
    [Table("competition_riddle")]
    public class CompetitionRiddleModel
    {
        [Key]
        [Column("id")]
        public int ID { get; set; }

        [Column("competition_id")]
        public int CompetitionID { get; set; }

        [ForeignKey(nameof(CompetitionID))]
        public CompetitionModel Competition { get; set; } = null!;

        [Column("question")]
        [MaxLength(256)]
        public string Question { get; set; } = string.Empty;

        [Column("answer")]
        [MaxLength(256)]
        public string Answer { get; set; } = string.Empty;

        [Column("other_answers", TypeName = "jsonb")]
        public string? OtherAnswers { get; set; }

        // Utility methods for conversion between domain and EF model
        public static CompetitionRiddleModel FromDomain(RiddleInfo riddle)
        {
            return new CompetitionRiddleModel
            {
                ID = riddle.Id ?? 0,
                CompetitionID = riddle.CompetitionID,
                Question = riddle.Question,
                Answer = riddle.TrueAnswer?.TextAnswer ?? string.Empty,
                OtherAnswers = JsonSerializer.Serialize(
                    riddle.PossibleAnswers.Select(x=>x.TextAnswer).ToArray()
                )
            };
        }

        public RiddleInfo ToDomain()
        {
            var otherAnswers = new List<RiddleAnswer>();
            if (!string.IsNullOrEmpty(OtherAnswers))
            {
                try
                {
                    var x = JsonSerializer.Deserialize<string[]>(OtherAnswers!) ?? Array.Empty<string>();
                    otherAnswers = x.Select(x => new RiddleAnswer(x)).ToList();
                }
                catch
                {
                    otherAnswers = new();
                }
            }

            var trueAnswer = new RiddleAnswer(Answer);
            var possibleAnswers = new List<RiddleAnswer>(otherAnswers);
            return new RiddleInfo(CompetitionID, Question, possibleAnswers, trueAnswer, ID);
        }
    }
}
