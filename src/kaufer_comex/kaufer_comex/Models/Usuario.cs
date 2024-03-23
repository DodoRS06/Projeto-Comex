using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nome do Funcionário (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Nome do Funcionário!")]
        public string NomeFuncionario { get; set; }

        [Display(Name = "E-mail (*)")]
        public string Email { get; set; }

        [Display(Name = "Senha (*)")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "CPF (*)")]
        [Required(ErrorMessage = "Obrigatório informar o CPF do Funcionário!")]
        public int CPF { get; set; }

        [Display(Name = "Tipo de Usuário (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Tipo de Usuário!")]
        public Perfil Perfil { get; set; }

        //public virtual  ICollection<UsuarioProcesso> UsuarioProcessos { get; set; } 

    }
    public enum Perfil
    {
        Admin,
        User,
    }
}
