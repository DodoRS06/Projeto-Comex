using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Despacho")]
    public class Despacho
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório informar número DUE.")]
        [Display(Name = "Número DUE (*)")]
        public string NumeroDue { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Data DUE (*)")]
        [Required(ErrorMessage = "Obrigatório informar a data DUE.")]
        public DateTime DataDue { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Data de Exportação (*)")]
        [Required(ErrorMessage = "Obrigatório informar a data de exportação.")]
        public DateTime DataExportacao { get; set; }


        [Display(Name = "Conhecimento de Embarque (*)")]
        public string ConhecimentoEmbarque { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Data de Conhecimento (*)")]
        [Required(ErrorMessage = "Obrigatório informar a data de conhecimento.")]
        public DateTime DataConhecimento { get; set; }


        [Display(Name = "Tipo (*)")]
        [Required(ErrorMessage = "Obrigatório informar o tipo.")]
        public Tipo Tipo { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        [Display(Name = "Data da Averbação (*)")]
        [Required(ErrorMessage = "Obrigatório informar a data de averbação.")]
        public DateTime DataAverbacao { get; set; }


        [Display(Name = "Código do País (*)")]
        [Required(ErrorMessage = "Obrigatório informar o código do país.")]
        public int CodPais { get; set; }


        [Display(Name = "Parametrização (*)")]
        [Required(ErrorMessage = "Obrigatório informar a parametrização.")]
        public Parametrizacao Parametrizacao { get; set; }


        public int ProcessoId { get; set; }

        [ForeignKey("ProcessoId")]
        public Processo Processo { get; set; }


    }

    public enum Tipo
    {
        CRT,
        AWB,
        BL
    }

    public enum Parametrizacao
    {
        Verde,
        Laranja,
        Vermelho
    }
}

