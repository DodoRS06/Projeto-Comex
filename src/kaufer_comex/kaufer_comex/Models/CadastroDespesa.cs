using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("CadastroDespesa")]
    public class CadastroDespesa
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Despesa (*)")]
		[Required(ErrorMessage = "Obrigatório informar o nome da despesa!")]
		public string NomeDespesa { get; set; }

		//[ForeignKey("DCEId")]
		//public DCE DCE { get; set; }

		public virtual ICollection<CadastroDespesaDCE> CadastroDespesaDCEs { get; set; }
	}
}
