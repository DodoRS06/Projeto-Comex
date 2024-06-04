using System.ComponentModel.DataAnnotations;

namespace kaufer_comex.Models
{
    public class AdicionaItemView
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Obrigatório selecionar um item.")]
        [Display(Name = "Item")]
        public int ItemId { get; set; }

        [Required(ErrorMessage = "Obrigatório adicionar a quantidade.")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, int.MaxValue, ErrorMessage = "Valores maiores que 0")]

        public int Quantidade { get; set; }

        public int EmbarqueRodoviarioId { get; set; }
    }
}
