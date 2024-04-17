
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    [Table("Notas")]
    public class Nota
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Número Nf (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public int NumeroNf { get; set; }

        [Display(Name = "Emissão (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public DateTime Emissao{ get; set; }

        [Display(Name = "Base Nota (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public DateTime BaseNota { get; set; }

        [Display(Name = "Valor Fob (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorFob { get; set; }

        [Display(Name = "Valor Frete (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorFrete { get; set; }

        [Display(Name = "Valor Seguro (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorSeguro { get; set; }

        [Display(Name = "Valor Cif (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float ValorCif { get; set; }

        [Display(Name = "Peso Liq (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoLiq { get; set; }

        [Display(Name = "Peso Bruto (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float PesoBruto { get; set; }

        [Display(Name = "Taxa Cambial (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public float TaxaCambial { get; set; }

        [Display(Name = "Certificado Qualidade (*)")]
        [Required(ErrorMessage = "Obrigatório")]
        public string CertificadoQualidade { get; set; }

        public int EmbarqueRodoviarioId { get; set; }

        [ForeignKey("EmbarqueRodoviarioId")]
        public EmbarqueRodoviario EmbarqueRodoviario { get; set; }

        public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo Veiculo { get; set; }

       // public virtual ICollection<NotaItem> NotaItem { get; set; }
      
    }
}  


