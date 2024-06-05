
using kaufer_comex.Migrations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Emissao { get; set; }

        [Display(Name = "Base Nota (*)")]
        [Required(ErrorMessage = "Obrigatório informar a data.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime BaseNota { get; set; }

        [Display(Name = "Valor Fob (*)")]
        [Required(ErrorMessage = "Obrigatório informar o valor Fob.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorFob { get; set; }

        [Display(Name = "Valor Frete (*)")]
        [Required(ErrorMessage = "Obrigatório informar o valor do frete.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorFrete { get; set; }

        [Display(Name = "Valor Seguro (*)")]
        [Required(ErrorMessage = "Obrigatório informar o valor do seguro.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorSeguro { get; set; }

        [Display(Name = "Valor Cif (*)")]
        [Required(ErrorMessage = "Obrigatório informar o valor Cif.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorCif { get; set; }

        [Display(Name = "Peso Liq (*)")]
        [Required(ErrorMessage = "Obrigatório informar o peso líquido.")]
        public float PesoLiq { get; set; }

        [Display(Name = "Peso Bruto (*)")]
        [Required(ErrorMessage = "Obrigatório informar o peso bruto.")]
        public float PesoBruto { get; set; }

        [Display(Name = "Taxa Cambial (*)")]
        [Required(ErrorMessage = "Obrigatório informar a taxa cambial.")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal TaxaCambial { get; set; }

        [Display(Name = "Certificado Qualidade (*)")]
        public string CertificadoQualidade { get; set; }


        [Display(Name = "Embarque Rodoviário (*)")]
        public int EmbarqueRodoviarioId { get; set; }

        [ForeignKey("EmbarqueRodoviarioId")]
        public EmbarqueRodoviario EmbarqueRodoviario { get; set; }

        [Display(Name = "Veículo (*)")]
        public int VeiculoId { get; set; }

        [ForeignKey("VeiculoId")]
        public Veiculo Veiculo { get; set; }

        [Display(Name = "Quantidade total")]
        public double QuantidadeTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Valor total")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorTotalNota { get; set; }

        public virtual List<NotaItem> NotaItem { get; set; }

        [NotMapped]
        public List<NotaItemTemp> NotaItemTemps { get; set; }
    }
}


