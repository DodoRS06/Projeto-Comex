using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Despesa-DCE")]
    public class CadastroDespesaDCE
    {
        public int CadastroDespesaId { get; set; }

        public CadastroDespesa CadastroDespesa { get; set; }

        public int DCEId { get; set; }

        public DCE DCE { get; set; }


    }
}
