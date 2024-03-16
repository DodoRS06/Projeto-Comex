/*using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace kaufer_comex.Models
{

    [Table("Processo")]
    public class Processo
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Código de processo de exportação (*)")]
        [Required]
        public string CodExportacao { get; set; }

        [Display(Name = "Exportador (*)")]
        [Required]
        public int ExportadorId { get; set; }

        [Display(Name = "Importador (*)")]
        [Required]
        public int ImportadorId { get; set; }

        [Display(Name = "Modal (*)")]
        [Required]
        public Modal Modal { get; set; }

        [Display(Name = "Incoterm (*)")]
        [Required]
        public Incoterm Incoterm { get; set; }

        [Display(Name = "Destino (*)")]
        [Required]
        public int DestinoId { get; set; }

        [ForeignKey("DestinoId")]
        public Destino Destino { get; set; }

        [Display(Name = "Agente de Carga (*)")]
        [Required]
        public int AgenteDeCargaId { get; set; }

       [ForeignKey("AgenteDeCargaId")]
        public AgenteDeCarga AgenteDeCarga { get; set; }

        [Display(Name = "Fronteira (*)")]
        [Required]
        public int FronteiraId { get; set; }

        [ForeignKey("FronteiraId")]
        public Fronteira Fronteira { get; set; }

        [Display(Name = "Despachante (*)")]
        [Required]
        public int DespachanteId { get; set; }

        [ForeignKey("DespachanteId")]
        public Despachante Despachante { get; set; }

        [Display(Name = "Vendedor (*)")]
        [Required]
        public int VendedorId { get; set; }

       [ForeignKey("VendedorId")]
        public Vendedor Vendedor { get; set; }

        [Display(Name = "Status (*)")]
        [Required]
        public int StatusId { get; set; }

       [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [Display(Name = "Proforma(*)")]
        [Required]
        public string Proforma { get; set; }

        [Display(Name = "Data de Início do Processo (*)")]
        public DateTime DataInicioProcesso { get; set; }

        [Display(Name = "Previsão de produção (*)")]
        public DateTime PrevisaoProducao { get; set; }

        [Display(Name = "Previsão de pagamento (*)")]
        public DateTime PrevisaoPagamento { get; set; }

        [Display(Name = "Previsão Coleta (*)")]
        public DateTime PrevisaoColeta { get; set; }

        [Display(Name = "Previsão Cruze (*)")]
        public DateTime PrevisaoCruze { get; set; }

        [Display(Name = "Previsão de entrega (*)")]
        public DateTime PrevisaoEntrega { get; set; }

        [Display(Name = "Observações (*)")]
        public string Observacoes { get; set; }

        [Display(Name = "Pedidos Relacionados (*)")]
        public string PedidosRelacionados { get; set; }

        [Display(Name = "Despacho (*)")]
        [Required]
        public int DespachoId { get; set; }

        [ForeignKey("DespachoId")]
        public Despacho Despacho { get; set; }

        [Display(Name = "Documentos (*)")]
        [Required]
        public int DocumentoId { get; set; }

        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }

        [Display(Name = "Valores do Processo (*)")]
        [Required]
        public int ValorProcessoId { get; set; }

        [ForeignKey("ValorProcessoId")]
        public ValorProcessoId ValorProcessoId { get; set; }

        [Display(Name = "Embarque Rodoviário (*)")]
        [Required]
        public int EmbarqueRodoviarioId { get; set; }

       [ForeignKey("EmbarqueRodoviarioId")]
        public EmbarqueRodoviario EmbarqueRodoviario { get; set; }

        [Display(Name = "DCE (*)")]
        [Required]
        public int DCE_Id { get; set; }

       [ForeignKey("DCE_Id")]
        public DCE DCE { get; set; }
    }

    public enum Modal
    {
        Rodoviario,
        Aereo,
        Maritimo
    }

    public enum Incoterm
    {
        CFR,
        CIF,
        CIP,
        CPT,
        DAP,
        DAT,
        DDP,
        EXW,
        FAS,
        FCA,
        FOB
    }
}
 */