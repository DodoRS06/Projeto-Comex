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

        [Display(Name = "Seguro Internaciona")]
        public float SeguroInternaciona { get; set; }

        [Display(Name = "Valor Total (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorTotalCif { get; set; }

        public Moeda Moeda { get; set; }


    }

    public enum Moeda
    {
        Real,
        Dólar,
        Euro,
    }
}
