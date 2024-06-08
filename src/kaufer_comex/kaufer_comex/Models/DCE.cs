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

        [Display(Name = "Fornecedor (*)")]
        [Required(ErrorMessage = "Obrigatório informar o fornecedor!")]
        public int FornecedorServicoId { get; set; }

        [Display(Name = "Valor (*)")]
        [Required(ErrorMessage = "Obrigatório informar o valor!")]
        public float Valor { get; set; }

        [Display(Name = "Observações")]
        public string Observacao { get; set; }

        [NotMapped]
        public string CadastroDespesaNome { get; set; }

        [NotMapped]
        public string FornecedorServicoNome { get; set; }

        public List<CadastroDespesaDCE> CadastroDespesas { get; set; }
        public List<FornecedorServicoDCE> FornecedorServicos { get; set; }

        [ForeignKey("ProcessoId")]
        public int ProcessoId { get; set; }
        public Processo Processo { get; set; }
    }
}
