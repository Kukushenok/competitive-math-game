using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoriesRealisation.Models
{
    public class OneToOneEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [ForeignKey(nameof(Id))]
        public virtual T? Model { get; set; }
    }
}
