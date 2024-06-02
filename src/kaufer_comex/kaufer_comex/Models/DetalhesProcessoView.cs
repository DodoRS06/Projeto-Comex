using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace kaufer_comex.Models
{
    public class DetalhesProcessoView
    {
        public int Id { get; set; }

        [Display(Name = "Código de processo de exportação (*)")]
        public string CodProcessoExportacao { get; set; }

        [Display(Name = "Exportador (*)")]
        public int ExportadorId { get; set; }

        [Display(Name = "Importador (*)")]
        public int ImportadorId { get; set; }

        [Display(Name = "Modal (*)")]
        public Modal Modal { get; set; }

        [Display(Name = "Incoterm (*)")]
        public Incoterm Incoterm { get; set; }

        [Display(Name = "Destino (*)")]
        public int DestinoId { get; set; }

        [ForeignKey("DestinoId")]
        public Destino Destino { get; set; }

        [Display(Name = "Responsável (*)")]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        [Display(Name = "Fronteira (*)")]
        public int FronteiraId { get; set; }

        [ForeignKey("FronteiraId")]
        public Fronteira Fronteira { get; set; }

        [Display(Name = "Despachante (*)")]
        public int DespachanteId { get; set; }

        [ForeignKey("DespachanteId")]
        public Despachante Despachante { get; set; }

        [Display(Name = "Vendedor (*)")]
        public int VendedorId { get; set; }

        [ForeignKey("VendedorId")]
        public Vendedor Vendedor { get; set; }

        [Display(Name = "Status (*)")]
        public int StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status Status { get; set; }

        [Display(Name = "Proforma(*)")]
        public string Proforma { get; set; }

        [Display(Name = "Data de Início do Processo (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataInicioProcesso { get; set; }

        [Display(Name = "Previsão de produção (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PrevisaoProducao { get; set; }

        [Display(Name = "Previsão de pagamento (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PrevisaoPagamento { get; set; }

        [Display(Name = "Previsão Coleta (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PrevisaoColeta { get; set; }

        [Display(Name = "Previsão Cruze (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PrevisaoCruze { get; set; }

        [Display(Name = "Previsão de entrega (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime PrevisaoEntrega { get; set; }

        [Display(Name = "Observações (*)")]
        public string Observacoes { get; set; }

        [Display(Name = "Pedidos Relacionados (*)")]
        public string PedidosRelacionados { get; set; }


        public virtual ICollection<DCE> DCES { get; set; }

        public virtual ICollection<ProcessoExpImp> ExpImps { get; set; }

        public List<Despacho> Despachos { get; set; }

        public List<Documento> Documentos { get; set; }

        public List<EmbarqueRodoviario> EmbarquesRodoviarios { get; set; }

        public List<ValorProcesso> ValorProcessos { get; set; }

        public List<Nota> Notas { get; set; }

        public List<Veiculo> Veiculos { get; set; }

    }
}
