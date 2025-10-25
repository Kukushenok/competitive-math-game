using System.ComponentModel.DataAnnotations.Schema;
using CompetitiveBackend.Core.Objects.Riddles;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RepositoriesRealisation.Models
{
    [Table("competition")]
    public class RiddleGameSettingsModel : OneToOneEntity<CompetitionModel>
    {
        [Column("score_on_right_answer")]
        public int ScoreOnRightAnswer { get; set; }

        [Column("score_on_bad_answer")]
        public int ScoreOnBadAnswer { get; set; }

        [Column("total_riddles")]
        public int TotalRiddles { get; set; }

        [Column("time_limit")]
        public TimeSpan? TimeLimit { get; set; }

        [Column("time_linear_bonus")]
        public int TimeLinearBonus { get; set; }

        public RiddleGameSettingsModel()
        {
        }

        public RiddleGameSettingsModel(int competitionId, RiddleGameSettings settings)
        {
            Id = competitionId;
            ScoreOnRightAnswer = settings.ScoreOnRightAnswer;
            ScoreOnBadAnswer = settings.ScoreOnBadAnswer;
            TotalRiddles = settings.TotalRiddles;
            TimeLimit = settings.TimeLimit;
            TimeLinearBonus = settings.TimeLinearBonus;
        }

        public RiddleGameSettings ToCoreModel()
        {
            return new RiddleGameSettings(
                ScoreOnRightAnswer,
                ScoreOnBadAnswer,
                TotalRiddles,
                TimeLimit,
                TimeLinearBonus);
        }

        public void UpdateFromCoreModel(RiddleGameSettings settings)
        {
            ScoreOnRightAnswer = settings.ScoreOnRightAnswer;
            ScoreOnBadAnswer = settings.ScoreOnBadAnswer;
            TotalRiddles = settings.TotalRiddles;
            TimeLimit = settings.TimeLimit;
            TimeLinearBonus = settings.TimeLinearBonus;
        }
    }
}
