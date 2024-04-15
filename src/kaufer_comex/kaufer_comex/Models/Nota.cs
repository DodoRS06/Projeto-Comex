
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Notas")]
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public int NumeroNf { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public DateTime Emissao{ get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public DateTime BaseNota { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float ValorFob { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float ValorFrete { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float ValorSeguro { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float ValorCif { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float PesoLiq { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float PesoBruto { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public float TaxaCambial { get; set; }

        [Required(ErrorMessage = "Obrigatório")]
        public string CertificadoQualidade { get; set; }

        public int VeiculoId { get; set; } 
        
        public  Veiculo  Veiculo { get; set; } 

       // public virtual ICollection<NotaItem> NotaItem { get; set; }
      
    }
}

