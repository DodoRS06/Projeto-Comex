using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("CadastroDespesa")]
    public class CadastroDespesa
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Despesa (*)")]
        [Required]
        public string NomeDespesa { get; set; }

        [ForeignKey("DCEId")]
        public DCE DCE { get; set; }
    }
}
