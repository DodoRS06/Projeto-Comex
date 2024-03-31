using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table ("Fronteiras")]
    public class Fronteira
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        public string NomeFronteira { get; set; }

        //public virtual ICollection<Processo> Processos { get; set; }


    }
}

