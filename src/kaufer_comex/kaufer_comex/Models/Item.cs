
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Itens")]
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

        [Display(Name = "Espessura")]
        [Required(ErrorMessage = "Obrigatório")]
        public float Espessura { get; set; }

        [Display(Name = "Área M2")]

        public float AreaM2 { get; set; }

        [Display(Name = "Diâmetro/Altura")]
        [Required(ErrorMessage = "Obrigatório")]
        public float DiametroAltura { get; set; }

        [Display(Name = "Diâmetro/Comprimento")]
        [Required(ErrorMessage = "Obrigatório")]
        public float DiametroComprimento { get; set; }

        [Display(Name = "Largura Aparente")]
        [Required(ErrorMessage = "Obrigatório")]
        public float LarguraAparente { get; set; }

        [Display(Name = "Volume M3")]

        public float VolumeM2 { get; set; }

        [Display(Name = "Peso Líquido (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoLiquido { get; set; }

        [Display(Name = "Peso Bruto (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoBruto { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Preço (*)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        public virtual ICollection<NotaItem> NotaItem { get; set; }

    }
}
