using System.ComponentModel.DataAnnotations.Schema;
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
        public ExpImp Exportador { get; set; }

        [Display(Name = "Importador (*)")]
        [Required]
        public ExpImp Importador { get; set; }

        [Display(Name = "Modal (*)")]
        [Required]
        public Modal Modal { get; set; }

        [Display(Name = "Incoterm (*)")]
        [Required]
        public Incoterm Incoterm { get; set; }

        [Display(Name = "Destino (*)")]
        [Required]
        public Destino Destino { get; set; }

        [Display(Name = "Agente de Carga (*)")]
        [Required]
        public AgenteDeCarga AgenteDeCarga { get; set; }

        [Display(Name = "Fronteira (*)")]
        [Required]
        public Fronteira Fronteira { get; set; }

        [Display(Name = "Despachante (*)")]
        [Required]
        public Despachante Despachante { get; set; }

        [Display(Name = "Vendedor (*)")]
        [Required]
        public Vendedor Vendedor { get; set; }

        [Display(Name = "Status (*)")]
        [Required]
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
    }

    public enum Modal
    {

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
