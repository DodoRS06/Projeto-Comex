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
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? ValorExw { get; set; }

        [Display(Name = " Valor Fob/Fca ")]
        [Required(ErrorMessage = "Obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? ValorFobFca { get; set; }

        [Display(Name = "Frete Internacional ")]
        [Required(ErrorMessage = "Obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? FreteInternacional { get; set; }

        [Display(Name = "Seguro Internacional")]
        [Required(ErrorMessage = "Obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? SeguroInternaciona { get; set; }

        [Display(Name = "Valor Total (*)")]
        [Column(TypeName = "decimal(18,2)")]
        [Required(ErrorMessage = "Obrigatório")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal? ValorTotalCif { get; set; }


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
        Yuan,
        Outras
    }
}
