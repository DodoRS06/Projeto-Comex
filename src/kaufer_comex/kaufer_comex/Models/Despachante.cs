using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace kaufer_comex.Models
{
    [Table("Despachantes")]
    public class Despachante
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Despachante (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Despachante!")]
        public string NomeDespachante { get; set; }
    }
}
