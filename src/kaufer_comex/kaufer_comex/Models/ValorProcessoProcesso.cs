using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("ValorProcessoProcessos")]
    public class ValorProcessoProcesso
    {
        public int ValorProcessoId { get; set; }

        public ValorProcesso ValorProcesso { get; set; }

        public int ProcessoId { get; set; }

        public Processo Processo { get; set; }

    }
}