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

        [Required(ErrorMessage = "Obrigatório informar a data DUE.")]
        [Display(Name = "Data DUE (*)")]
        public DateTime DataDue { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a data da exportação.")]
        [Display(Name = "Data de Exportação (*)")]
        public DateTime DataExportacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar conhecimento de embarque.")]
        [Display(Name = "Conhecimento de Embarque (*)")]
        public string ConhecimentoEmbarque { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a data de conhecimento.")]
        [Display(Name = "Data de Conhecimento (*)")]
        public DateTime DataConhecimento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o tipo.")]
        [Display(Name = "Tipo (*)")]
        public Tipo Tipo { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a data da averbação.")]
        [Display(Name = "Data da Averbação (*)")]
        public DateTime DataAverbacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o código do País.")]
        [Display(Name = "Código do País (*)")]
        public int CodPais { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a parametrização.")]
        [Display(Name = "Parametrização (*)")]
        public Parametrizacao Parametrizacao { get; set; }

        [Required(ErrorMessage = "Obrigatório informar o código do processo.")]
        [Display(Name = "Código de processo (*)")]
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

