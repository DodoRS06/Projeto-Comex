using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
	[Table("DCETemp")]
	public class DCETemp
	{
		[Key]
		public int Id { get; set; }
		public int CadastroDespesaId { get; set; }
		public string CadastroDespesaNome { get; set; }
		public int FornecedorServicoId { get; set; }
		public string FornecedorServicoNome { get; set; }
		public float Valor { get; set; }
		public string Observacao { get; set; }
		public int ProcessoId { get; set; }
	}
}
