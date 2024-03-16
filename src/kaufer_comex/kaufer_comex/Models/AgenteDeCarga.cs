using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("AgenteDeCargas")]
    public class AgenteDeCarga

    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Agente de Carga (*)")]
        [Required(ErrorMessage ="Obrigatório selecionar o Agente de Carga!")]
        public string NomeAgenteCarga { get; set; }
    }
}
