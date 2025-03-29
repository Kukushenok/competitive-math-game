using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Name { get; set; }

        [Column("email", TypeName = "varchar(32)")]
        public string? Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("privilegy_level")]
        public int AccountPrivilegyLevel { get; set; }
        [Column("description", TypeName = "varchar(128)")]
        public string? Description { get; set; }

        [Column("profile_image", TypeName = "blob")]
        public byte[]? ProfileImage { get; set; }
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
        public Account ToCoreAccount()
        {
            return new Account(Name, PasswordHash, Email, Id);
        }
        public PlayerProfile ToCoreProfile()
        {
            return new PlayerProfile(Name, Description, Id);
        }
    }
}
