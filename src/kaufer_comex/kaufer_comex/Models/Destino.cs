using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Destino")]
    public class Destino
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NomePais { get; set; }

        public virtual ICollection<Processo> Processos { get; set; }
    }
}
