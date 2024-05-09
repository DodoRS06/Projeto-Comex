using Microsoft.EntityFrameworkCore;
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
        [Required(ErrorMessage = "Obrigatório informar o email!")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|org|net|gov|br)$", ErrorMessage = "Por favor, digite um e-mail válido!")]
        public string Email { get; set; }

        [Display(Name = "Senha (*)")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }


        [Display(Name = "CPF (*)")]
        [Required(ErrorMessage = "Obrigatório informar o CPF do Funcionário!")]
        [RegularExpression(@"\d{3}\.\d{3}.\d{3}-\d{2}", ErrorMessage = "Por favor, digite um CPF válido!")]
        public string CPF { get; set; }

        [Display(Name = "Tipo de Usuário (*)")]
        [Required(ErrorMessage = "Obrigatório informar o Tipo de Usuário!")]
        public Perfil Perfil { get; set; }


    }
    public enum Perfil
    {
        Admin,
        User,
    }
}
