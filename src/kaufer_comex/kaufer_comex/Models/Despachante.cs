using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Despachante")]
    public class Despachante
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Despachante (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Despachante !")]
        public string NomeDespachante { get; set; }
    }
}

