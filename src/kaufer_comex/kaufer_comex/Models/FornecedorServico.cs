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
        [Required]
        public string Nome { get; set; }

        [Display(Name = "Tipo de serviço (*)")]
        [Required]
        public TipoServico TipoServico { get; set; }

        [ForeignKey("DCEId")]
        public DCE DCE { get; set; }
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