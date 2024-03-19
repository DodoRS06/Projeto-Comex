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
        [Required(ErrorMessage = "Obrigatório informar o código de exportação.")]
        public string CodExportacao { get; set; }

        [Display(Name = "Exportador (*)")]
        [Required(ErrorMessage = "Obrigatório informar o código do exportador.")]
        public int ExportadorId { get; set; }

        [Display(Name = "Importador (*)")]
        [Required(ErrorMessage = "Obrigatório informar o código do importador.")]
        public int ImportadorId { get; set; }

        [Display(Name = "Modal (*)")]
        public Modal Modal { get; set; }

        [Display(Name = "Incoterm (*)")]
        [Required(ErrorMessage = "Obrigatório informar Incoterm.")]
        public Incoterm Incoterm { get; set; }

        [Display(Name = "Destino (*)")]
        [Required(ErrorMessage = "Obrigatório informar o destino.")]
        public int DestinoId { get; set; }

        [ForeignKey("DestinoId")]
        public Destino Destino { get; set; }

        [Display(Name = "Agente de Carga (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Agente de carga.")]
        public int AgenteDeCargaId { get; set; }

        [ForeignKey("AgenteDeCargaId")]
        public AgenteDeCarga AgenteDeCarga { get; set; }

        [Display(Name = "Fronteira (*)")]
        [Required(ErrorMessage = "Obrigatório informar a fronteira.")]
        public int FronteiraId { get; set; }

        [ForeignKey("FronteiraId")]
        public Fronteira Fronteira { get; set; }

        [Display(Name = "Despachante (*)")]
        [Required(ErrorMessage = "Obrigatório informar o despachante.")]
        public int DespachanteId { get; set; }

        [ForeignKey("DespachanteId")]
        public Despachante Despachante { get; set; }

        [Display(Name = "Vendedor (*)")]
        [Required(ErrorMessage = "Obrigatório informar o vendedor.")]
        public int VendedorId { get; set; }

        [ForeignKey("VendedorId")]
        public Vendedor Vendedor { get; set; }

        [Display(Name = "Status (*)")]
        [Required(ErrorMessage = "Obrigatório informar o status.")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [Display(Name = "Proforma(*)")]
        [Required(ErrorMessage = "Obrigatório informar Proforma.")]
        public string Proforma { get; set; }

        [Display(Name = "Data de Início do Processo (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime DataInicioProcesso { get; set; }

        [Display(Name = "Previsão de produção (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime PrevisaoProducao { get; set; }

        [Display(Name = "Previsão de pagamento (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime PrevisaoPagamento { get; set; }

        [Display(Name = "Previsão Coleta (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime PrevisaoColeta { get; set; }

        [Display(Name = "Previsão Cruze (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime PrevisaoCruze { get; set; }

        [Display(Name = "Previsão de entrega (*)")]
        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        public DateTime PrevisaoEntrega { get; set; }

        [Display(Name = "Observações (*)")]
        public string Observacoes { get; set; }

        [Display(Name = "Pedidos Relacionados (*)")]
        public string PedidosRelacionados { get; set; }

        [Display(Name = "Despacho (*)")]
        [Required(ErrorMessage = "Obrigatório informar o código de despacho.")]
        public int DespachoId { get; set; }

        [ForeignKey("DespachoId")]
        public Despacho Despacho { get; set; }

        [Display(Name = "Documentos (*)")]
        [Required(ErrorMessage = "Obrigatório informar o documento.")]
        public int DocumentoId { get; set; }

        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }

        [Display(Name = "Valores do Processo (*)")]
        [Required]
        public int ValorProcessoId { get; set; }

        [ForeignKey("ValorProcessoId")]
        public ValorProcessoId ValorProcessoId { get; set; }

        [Display(Name = "Embarque Rodoviário (*)")]
        [Required(ErrorMessage = "Obrigatório informar os dados de embarque.")]
        public int EmbarqueRodoviarioId { get; set; }

        [ForeignKey("EmbarqueRodoviarioId")]
        public EmbarqueRodoviario EmbarqueRodoviario { get; set; }

        [Display(Name = "DCE (*)")]
        [Required]
        public int DCEId { get; set; }

        [ForeignKey("DCEId")]
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