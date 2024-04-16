
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Item")]
    public class Item
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Código Produto (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public string CodigoProduto { get; set; }

        [Display(Name = "Descrição Produto (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public string DescricaoProduto { get; set; }

        [Display(Name = "Família (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public string Familia { get; set; }

        [Display(Name = "Largura (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float Largura { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Comprimento { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float Expressura { get; set; }

        [Display(Name = "Área M2 (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float AreaM2 { get; set; }

        [Display(Name = "Diâmetro (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float Diametro { get; set; }

        [Display(Name = "Largura Aparente (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float LarguraAparente { get; set; }

        [Display(Name = "Volume M2 (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float VolumeM2 { get; set; }

        [Display(Name = "Peso Líquido (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoLiquido { get; set; }

        [Display(Name = "Peso Bruto (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoBruto { get; set; }

      //  public virtual ICollection<NotaItem> NotaItem { get; set; }

    }
}
