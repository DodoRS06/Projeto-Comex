using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Processo> Processos { get; set; }

        public DbSet<AgenteDeCarga> AgenteDeCargas{ get; set; }

        public DbSet<Despachante> Despachantes { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        //public DbSet<Despacho> Despachos { get; set;}

       // public DbSet<ProcessoExpImp> ProcessoExpImps { get; set; }
    }
}
