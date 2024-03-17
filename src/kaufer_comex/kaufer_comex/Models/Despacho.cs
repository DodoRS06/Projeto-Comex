/*using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Despacho")]
    public class Despacho
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Número DUE (*)")]
        public string NumeroDue { get; set; }

        [Required]
        [Display(Name = "Data DUE (*)")]
        public DateTime DataDue { get; set; }

        [Required]
        [Display(Name = "Data de Exportação (*)")]
        public DateTime DataExportacao { get; set; }

        [Required]
        [Display(Name = "Conhecimento de Embarque (*)")]
        public string ConhecimentoEmbarque { get; set; }

        [Required]
        [Display(Name = "Data de Conhecimento (*)")]
        public DateTime DataConhecimento { get; set; }

        [Required]
        [Display(Name = "Tipo (*)")]
        public Tipo Tipo { get; set; }

        [Required]
        [Display(Name = "Data da Averbação (*)")]
        public DateTime DataAverbacao { get; set; }

        [Required]
        [Display(Name = "Código do País (*)")]
        public int CodPais { get; set; }

        [Required]
        [Display(Name = "Parametrização (*)")]
        public Parametrizacao Parametrizacao { get; set; }

        [Required]
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
*/
