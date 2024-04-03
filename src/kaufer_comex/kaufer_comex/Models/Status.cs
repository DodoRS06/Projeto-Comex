
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Status")]
    public class Status
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Status Atual")]
        public string StatusAtual { get; set; }
    }
}
