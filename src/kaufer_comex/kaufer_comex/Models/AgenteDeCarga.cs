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
        [Required(ErrorMessage = "Obrigatório informar o Agente de Carga!")]
        public string NomeAgenteCarga { get; set; }

        [Display(Name = "Nome de Contato(*)")]
        [Required(ErrorMessage = "Obrigatório informar Proforma.")]
        public string Contato { get; set; }

        [Display(Name = "E-mail (*)")]
        [Required(ErrorMessage = "Obrigatório informar o email!")]
        public string Email { get; set; }

        [Display(Name = "Telefone (*)")]
        [Required(ErrorMessage = "Obrigatório informar o telefone!")]
        public string Telefone { get; set; }

    }
}
