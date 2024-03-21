/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace kaufer_comex.Models
{
    [Table ("Documentos")]
    public class Documento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar certificado origem")]
        public string CertificadoOrigem { get; set; }

        [Required(ErrorMessage = "Obrigatório informar certificado seguro")]
        public string CertificadoSeguro { get; set; }

        [Required(ErrorMessage = "Obrigatório informar data de envio")]

        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime DataEnvio { get; set; }

        [Required(ErrorMessage = "Obrigatório informar certificado tracking")]
        public string TrackinCourier { get; set;}

        [ForeignKey("ProcessoId")]
        public Processo Processo { get; set; }
    }
}
*/