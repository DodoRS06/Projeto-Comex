using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Destino")]
    public class Destino
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o destino.")]
        [Display(Name = "País")]
        public string NomePais { get; set; }

        public virtual ICollection<Processo> Processos { get; set; }
    }
}
