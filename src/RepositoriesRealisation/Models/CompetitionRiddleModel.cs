using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CompetitiveBackend.Core.Objects.Riddles;
using RepositoriesRealisation.Models;

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
                    riddle.PossibleAnswers.Select(x => x.TextAnswer).ToArray()),
            };
        }

        public RiddleInfo ToDomain()
        {
            var otherAnswers = new List<RiddleAnswer>();
            if (!string.IsNullOrEmpty(OtherAnswers))
            {
                try
                {
                    string[] x = JsonSerializer.Deserialize<string[]>(OtherAnswers!) ?? [];
                    otherAnswers = [.. x.Select(x => new RiddleAnswer(x))];
                }
                catch
                {
                    otherAnswers = [];
                }
            }

            var trueAnswer = new RiddleAnswer(Answer);
            var possibleAnswers = new List<RiddleAnswer>(otherAnswers);
            return new RiddleInfo(CompetitionID, Question, possibleAnswers, trueAnswer, ID);
        }
    }
}
