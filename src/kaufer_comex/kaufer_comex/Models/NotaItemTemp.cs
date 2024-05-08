using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    public class NotaItemTemp
    {
        [Key]
        public int NotaItemTempId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        [Display(Name = "Item")]
        public string Descricao { get; set; }


        [Required(ErrorMessage = "O campo é obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public double Quantidade { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get { return Preco * (decimal)Quantidade; } }

        public virtual Item Item { get; set; }

    }
}
