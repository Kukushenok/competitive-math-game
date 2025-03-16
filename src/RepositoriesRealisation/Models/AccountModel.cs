using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RepositoriesRealisation.DatabaseObjects
{
    [Table("account")]
    public class AccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Column("login", TypeName = "varchar(32)")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Name { get; set; }

        [Column("email", TypeName = "varchar(32)")]
        public string? Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("privilegy_level")]
        public int AccountPrivilegyLevel { get; set; }
        public AccountModel()
        {

        }

        public AccountModel(Account coreObject, Role role)
        {
            Id = coreObject.Id ?? 0;
            Name = coreObject.Login;
            Email = coreObject.Email;
            PasswordHash = coreObject.PasswordHash;
            AccountPrivilegyLevel = PrivilegyRoleResolver.Resolve(role);
        }
        public Account ToCore()
        {
            return new Account(Name, PasswordHash, Email, Id);
        }
    }
    public class PrivilegyRoleResolver
    {
        public static int Resolve(Role rl)
        {
            if (rl.IsPlayer()) return 0;
            if (rl.IsAdmin()) return 1;
            throw new Exception();
        }
        public static Role Resolve(int rl)
        {
            if (rl == 0) return new PlayerRole();
            if (rl == 1) return new AdminRole();
            throw new Exception();
        }
    }
}
