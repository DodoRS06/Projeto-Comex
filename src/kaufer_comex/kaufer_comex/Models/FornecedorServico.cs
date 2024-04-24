using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("FornecedorServico")]
    public class FornecedorServico
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Fornecedor (*)")]
		[Required(ErrorMessage = "Obrigatório informar o nome do fornecedor de serviços!")]
		public string Nome { get; set; }

        [Display(Name = "Tipo de serviço (*)")]
		[Required(ErrorMessage = "Obrigatório informar o tipo de serviço!")]
		public TipoServico TipoServico { get; set; }

		//[ForeignKey("DCEId")]
		//public DCE DCE { get; set; }

		public virtual ICollection<FornecedorServicoDCE> FornecedorServicoDCEs { get; set; }
	}
    public enum TipoServico
    {
        AgenteCarga,
        Transportadora,
        Armador,
        Armazem,
        Despachante,
        Impostos,
        Multas,
        Outros,
        OutrosServicos
    }
}