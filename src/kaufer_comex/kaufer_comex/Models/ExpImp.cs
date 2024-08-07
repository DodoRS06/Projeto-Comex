﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace kaufer_comex.Models
{
    [Table("ExpImp")]
    public class ExpImp
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Sigla (*)")]
        [Required(ErrorMessage = "Obrigatório informar a sigla!")]
        public string Sigla { get; set; }

        [Display(Name = "Tipo (*)")]
        [Required(ErrorMessage = "Obrigatório informar o tipo!")]
        public TipoExpImp TipoExpImp { get; set; }

        [Display(Name = "Nome (*)")]
        [Required(ErrorMessage = "Obrigatório informar o nome!")]
        public string Nome { get; set; }

        [Display(Name = "Endereço (*)")]
        [Required(ErrorMessage = "Obrigatório informar o endereço!")]
        public string Endereco { get; set; }

        [Display(Name = "Cidade (*)")]
        [Required(ErrorMessage = "Obrigatório informar a cidade!")]
        public string Cidade { get; set; }

        [Display(Name = "Estado (*)")]
        [Required(ErrorMessage = "Obrigatório informar o estado!")]
        public string Estado { get; set; }

        [Display(Name = "Pais (*)")]
        [Required(ErrorMessage = "Obrigatório informar o país!")]
        public string Pais { get; set; }

        [Display(Name = "CEP (*)")]
        [Required(ErrorMessage = "Obrigatório informar o CEP!")]
        public string Cep { get; set; }

        [Display(Name = "Telefone (*)")]
        [Required(ErrorMessage = "Obrigatório informar o telefone!")]
        public string Telefone { get; set; }

        [Display(Name = "Email (*)")]
        [Required(ErrorMessage = "Obrigatório informar o email!")]
        public string Email { get; set; }

        [Display(Name = "Número de Registro (*)")]
        [Required(ErrorMessage = "Obrigatório informar o número de registro!")]
        public string Cnpj { get; set; }

        [Display(Name = "Contato (*)")]
        [Required(ErrorMessage = "Obrigatório informar o contato!")]
        public string Contato { get; set; }

        [Display(Name = "Observações")]
        public string Observacoes { get; set; }

        public virtual ICollection<ProcessoExpImp> ProcessoExpImps { get; set; }
    }

    public enum TipoExpImp
    {
        Exportador,
        Importador
    }
}
