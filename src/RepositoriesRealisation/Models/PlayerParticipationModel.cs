using CompetitiveBackend.Core.Objects;
using Microsoft.EntityFrameworkCore;
using RepositoriesRealisation.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesRealisation.Models
{
    [Table("player_participation")]
    // СОСТАВНОЙ КЛЮЧ ОПРЕДЕЛЁН В BaseDbContext
    public class PlayerParticipationModel
    {
        public CompetitionModel Competition { get; set; }
        public AccountModel Account { get; set; }
        [ForeignKey("competition_id"), Column("competition_id", TypeName ="int")]
        public int CompetitionID { get; set; }
        [ForeignKey("account_id"), Column("account_id", TypeName = "int")]
        public int AccountID { get; set; }
        [Column("score", TypeName = "int")]
        public int Score { get; set; }
        public PlayerParticipationModel(int CompetitionID, int AccountID, int Score)
        {
            this.CompetitionID = CompetitionID;
            this.AccountID = AccountID;
            this.Score = Score;
        }
        public PlayerParticipationModel()
        {

        }
    }
}
