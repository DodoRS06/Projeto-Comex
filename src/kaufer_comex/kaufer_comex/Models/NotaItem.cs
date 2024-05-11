
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("NotaItens")]
    public class NotaItem
    {
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public int NotaId { get; set; }

        public Nota Nota { get; set; }

        [Required(ErrorMessage = "O campo é obrigatório")]
        public double Quantidade {  get; set; }

		[Column(TypeName = "decimal(18,2)")]
		[Required(ErrorMessage = "O campo é obrigatório")]
        public decimal Valor {  get; set; }

        
    }
}

