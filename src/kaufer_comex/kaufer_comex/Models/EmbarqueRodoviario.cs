using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace kaufer_comex.Models
{
    [Table("EmbarqueRodoviario")]
    public class EmbarqueRodoviario
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Transportadora: (*)")]
        [Required(ErrorMessage = "Obrigatório informar a transportadora")]
        public string Transportadora { get; set; }

        [Display(Name = "Data do Embarque: (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Obrigatório informar a data de embarque")]
        public DateTime DataEmbarque { get; set; }

        [Display(Name = "Transit Time: (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Transit Time")]
        public string TransitTime { get; set; }

        [Display(Name = "Chegada no Destino: (*)")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "Obrigatório informar a data de chegada")]
        public DateTime ChegadaDestino { get; set; }


        [Display(Name = "Agente de Carga: (*)")]
        [Required(ErrorMessage = "Obrigatório informar o agente.")]
        public int AgenteDeCargaId { get; set; }

        [ForeignKey("AgenteDeCargaId")]
        public AgenteDeCarga AgenteDeCarga { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o código.")]
        public int ProcessoId { get; set; }
        [ForeignKey("ProcessoId")]
        public Processo Processo { get; set; }


    }
}

