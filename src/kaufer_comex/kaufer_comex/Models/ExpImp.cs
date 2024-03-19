//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace kaufer_comex.Models
//{
//    [Table("ExpImp")]
//    public class ExpImp
//    {
//        [Key]
//        public int Id { get; set; }

//        [Display(Name = "Sigla (*)")]
//        [Required]
//        public string Sigla { get; set; }

//        [Display(Name = "Tipo (*)")]
//        [Required]
//        public Tipo Tipo { get; set; }

//        [Display(Name = "Nome (*)")]
//        [Required]
//        public string Nome { get; set; }

//        [Display(Name = "Endereço (*)")]
//        [Required]
//        public string Endereco { get; set; }

//        [Display(Name = "Cidade (*)")]
//        [Required]
//        public string Cidade { get; set; }

//        [Display(Name = "Estado (*)")]
//        [Required]
//        public string Estado { get; set; }

//        [Display(Name = "Pais (*)")]
//        [Required]
//        public string Pais { get; set; }

//        [Display(Name = "CEP (*)")]
//        [Required]
//        public string Cep { get; set; }

//        [Display(Name = "Telefone (*)")]
//        [Required]
//        public string Telefone { get; set; }

//        [Display(Name = "Email (*)")]
//        [Required]
//        public string Email { get; set; }

//        [Display(Name = "Cnpj (*)")]
//        [Required]
//        public string Cnpj { get; set; }

//        [Display(Name = "Contato (*)")]
//        [Required]
//        public string Contato { get; set; }

//        [Display(Name = "Obsercações (*)")]
//        [Required]
//        public string Observacoes { get; set; }

//        [ForeignKey("ProcessoId")]
//        public Processo Processo { get; set; }
//    }

//    public enum Tipo
//    {
//        Exportador,
//        Importador
//    }
//}
