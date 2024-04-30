using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("ValorProcessos")]
    public class ValorProcesso
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Valor Exw (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorExw { get; set; }

        [Display(Name = " Valor Fob/Fca ")]
        public float ValorFobFca { get; set; }

        [Display(Name = "Frete Internacional ")]
        public float FreteInternacional { get; set; }

        [Display(Name = "Seguro Internacional")]
        public float SeguroInternaciona { get; set; }

        [Display(Name = "Valor Total (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorTotalCif { get; set; }

		[Required(ErrorMessage = "Obrigatório informar o código do processo.")]
		[Display(Name = "Código de processo (*)")]
		public int ProcessoId { get; set; }

		[ForeignKey("ProcessoId")]
		public Processo Processo { get; set; }

		public Moeda Moeda { get; set; }


    }

    public enum Moeda
    {
        Real,
        Dólar,
        Euro,
    }
}
