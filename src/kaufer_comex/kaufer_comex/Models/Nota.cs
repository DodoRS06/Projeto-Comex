
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime BaseNota { get; set; }

        [Display(Name = "Valor Fob (*)")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorFob { get; set; }

        [Display(Name = "Valor Frete (*)")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorFrete { get; set; }

        [Display(Name = "Valor Seguro (*)")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorSeguro { get; set; }

        [Display(Name = "Valor Cif (*)")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ValorCif { get; set; }

        [Display(Name = "Peso Liq (*)")]
        public float PesoLiq { get; set; }

        [Display(Name = "Peso Bruto (*)")]
        public float PesoBruto { get; set; }

        [Display(Name = "Taxa Cambial (*)")]
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


