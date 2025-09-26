﻿using CompetitiveBackend.Core.Auth;
using CompetitiveBackend.Core.Objects;
using RepositoriesRealisation.DatabaseObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoriesRealisation.Models
{
    [Table("account")]
    public class AccountModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Column("login", TypeName = "varchar(32)")]
        public string Login { get; set; }
        [Column("email", TypeName = "varchar(32)")]
        public string? Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("privilegy_level")]
        public int AccountPrivilegyLevel { get; set; }
        public virtual AccountModelProfileImage ProfileImage { get; set; }
        public virtual PlayerProfileModel Profile { get; set; }
        public AccountModel()
        {

        }

        public AccountModel(Account coreObject, string PasswordHash, Role role)
        {
            Id = coreObject.Id ?? 0;
            Login = coreObject.Login;
            Profile = new PlayerProfileModel() { Id = coreObject.Id ?? 0, Name = coreObject.Login };
            Email = coreObject.Email;
            this.PasswordHash = PasswordHash;
            AccountPrivilegyLevel = PrivilegyRoleResolver.Resolve(role);
        }
        public Account ToCoreModel()
        {
            return new Account(Login, Email, Id);
        }

    }
    [Table("account")]
    public class PlayerProfileModel: OneToOneEntity<AccountModel>
    {
        [Column("username", TypeName = "varchar(32)")]
        public string Name { get; set; }
        [Column("description", TypeName = "varchar(128)")]
        public string? Description { get; set; }
        public PlayerProfile ToCoreModel()
        {
            return new PlayerProfile(Name, Description, Id);
        }
    }
    [Table("account")]
    public class AccountModelProfileImage: OneToOneEntity<AccountModel>
    {
        [Column("profile_image", TypeName = "bytea")]
        public byte[]? ProfileImage { get; set; }
    }
}
