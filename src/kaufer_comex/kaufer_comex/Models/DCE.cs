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
        [Required]
        public int CadastroDespesaId { get; set; }

        //[ForeignKey("CadastroDespesaId")]
        //public CadastroDespesa CadastroDespesa { get; set; }

        [Display(Name = "Fornecedor (*)")]
        [Required]
        public int FornecedorServicoId { get; set; }

        //[ForeignKey("FornecedorServicoId")]
        //public FornecedorServico FornecedorServico { get; set; }

        [Display(Name = "Valor (*)")]
        [Required]
        public float Valor { get; set; }

        [Display(Name = "Observação (*)")]
        [Required]
        public string Observacao { get; set; }

        public ICollection<CadastroDespesa> CadastroDespesas { get; set; }

        public ICollection<FornecedorServico> FornecedorServicos { get; set; }

        [ForeignKey("ProcessoId")]
        public Processo Processo { get; set; }
    }
}
