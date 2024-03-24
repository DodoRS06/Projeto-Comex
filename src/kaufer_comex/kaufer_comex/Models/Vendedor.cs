using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace kaufer_comex.Models
{
    [Table("Vendedores")]
    public class Vendedor
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Vendedor (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Vendedor!")]
        public string NomeVendedor { get; set; }
    }
}
