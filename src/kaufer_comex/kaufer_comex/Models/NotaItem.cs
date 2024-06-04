
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("NotaItens")]
    public class NotaItem
    {
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        public Item Item { get; set; }

        public int NotaId { get; set; }

        public Nota Nota { get; set; }

        public double Quantidade { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }


    }
}

