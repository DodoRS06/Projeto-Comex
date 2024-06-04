using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace kaufer_comex.Models
{
    public class DCEView
    {

        public int Id { get; set; }

        [Display(Name = "Despesa (*)")]
        public int CadastroDespesaId { get; set; }

        [Display(Name = "Fornecedor (*)")]
        public int FornecedorServicoId { get; set; }

        [Display(Name = "Valor (*)")]
        public float Valor { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [NotMapped]
        public string CadastroDespesaNome { get; set; }

        [NotMapped]
        public string FornecedorServicoNome { get; set; }

        public int ProcessoId { get; set; }

        public List<DCE> DCEs { get; set; }
    }
}
