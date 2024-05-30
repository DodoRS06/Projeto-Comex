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

        [Display(Name = "Nome de Contato(*)")]
        [Required(ErrorMessage = "Obrigatório informar Proforma.")]
        public string Contato { get; set; }

        [Display(Name = "E-mail (*)")]
        [Required(ErrorMessage = "Obrigatório informar o email!")]
        public string Email { get; set; }

        [Display(Name = "Telefone (*)")]
        [Required(ErrorMessage = "Obrigatório informar o telefone!")]
        public string Telefone { get; set; }
    }
}
