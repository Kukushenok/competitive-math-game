using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoriesRealisation.Models
{
    [Table("player_participation")]

    // СОСТАВНОЙ КЛЮЧ ОПРЕДЕЛЁН В BaseDbContext
    public class PlayerParticipationModel
    {
        public CompetitionModel Competition { get; set; } = null!;

        public PlayerProfileModel Account { get; set; } = null!;
        [ForeignKey("competition_id")]
        [Column("competition_id", TypeName = "int")]
        public int CompetitionID { get; set; }
        [ForeignKey("account_id")]
        [Column("account_id", TypeName = "int")]
        public int AccountID { get; set; }
        [Column("score", TypeName = "int")]
        public int Score { get; set; }

        [Column("last_update_time")]
        public DateTime LastUpdateTime { get; set; }
        public PlayerParticipationModel(int competitionID, int accountID, int score, DateTime lastUpdateTime)
        {
            CompetitionID = competitionID;
            AccountID = accountID;
            Score = score;
            LastUpdateTime = lastUpdateTime;
        }

        public PlayerParticipationModel()
        {
        }
    }
}
