using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("DCE")]
    public class DCE
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Despesa (*)")]
		[Required(ErrorMessage = "Obrigatório informar a despesa!")]
		public int CadastroDespesaId { get; set; }

        //[ForeignKey("CadastroDespesaId")]
        //public CadastroDespesa CadastroDespesa { get; set; }

        [Display(Name = "Fornecedor (*)")]
		[Required(ErrorMessage = "Obrigatório informar o fornecedor!")]
		public int FornecedorServicoId { get; set; }

        //[ForeignKey("FornecedorServicoId")]
        //public FornecedorServico FornecedorServico { get; set; }

        [Display(Name = "Valor (*)")]
		[Required(ErrorMessage = "Obrigatório informar o valor!")]
		public float Valor { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

		public virtual ICollection<CadastroDespesaDCE> CadastroDespesas { get; set; }
		public virtual ICollection<FornecedorServicoDCE> FornecedorServicos { get; set; }

        [ForeignKey("ProcessoId")]
        public Processo Processo { get; set; }
    }
}
