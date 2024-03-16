using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Processo> Processos { get; set; }

        public DbSet<AgenteDeCarga> AgenteDeCargas{ get; set; }
    }
}
