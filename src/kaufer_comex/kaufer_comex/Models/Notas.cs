using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Notas")]
    public class Notas
    {

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string CodigoProduto { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string DescricaoProduto { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string Familia { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Largura { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Comprimento { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Expressura { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float AreaM2 { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Diametro { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float LarguraAparente { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float VolumeM2 { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float PesoLiquido { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float PesoBruto { get; set; }
    }
}
