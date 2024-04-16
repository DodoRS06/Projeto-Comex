
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Veiculo")]
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string Motorista { get; set; }
    }
}

