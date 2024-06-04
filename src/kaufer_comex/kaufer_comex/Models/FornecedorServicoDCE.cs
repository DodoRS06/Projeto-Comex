using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Fornecedor-DCE")]
    public class FornecedorServicoDCE
    {
        public int FornecedorServicoId { get; set; }

        public FornecedorServico FornecedorServico { get; set; }

        public int DCEId { get; set; }

        public DCE DCE { get; set; }

    }
}
