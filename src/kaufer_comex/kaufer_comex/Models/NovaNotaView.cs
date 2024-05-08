using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace kaufer_comex.Models
{
    public class NovaNotaView
    {

        [Required(ErrorMessage = "O campo é obrigatório")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public Nota Nota { get; set; }

        public NotaItem NotaItem { get; set; }

		public AdicionaItem AdicionaItem { get; set; }

		[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double QuantidadeTotal { get { return NotaItens == null ? 0 : NotaItens.Sum(d => d.Quantidade); } }

		[Column(TypeName = "decimal(18,2)")]
		[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal ValorTotal { get { return NotaItens == null ? 0 : NotaItens.Sum(d => d.Valor); } }

		public List<NotaItem> NotaItens { get; set; }

		public List<Nota> Notas { get; set; }

		public List<AdicionaItem> Itens { get; set; }

	}
}
