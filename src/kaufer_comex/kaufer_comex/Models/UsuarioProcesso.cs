using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace kaufer_comex.Models
{
   
    [Table("UsuarioProcessos")]
    public class UsuarioProcesso
    {
            public int UsuarioId { get; set; }

            public Usuario Usuario { get; set; }

            public int ProcessoId { get; set; }

            public Processo Processo { get; set; }
        }
    }
