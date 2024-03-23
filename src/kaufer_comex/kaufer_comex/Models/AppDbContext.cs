using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace kaufer_comex.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        //public DbSet<Processo> Processos { get; set; }

        public DbSet<AgenteDeCarga> AgenteDeCargas{ get; set; }

        public DbSet<Despachante> Despachantes { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        //public DbSet<UsuarioProcesso> UsuariosProcesso{ get; set; }

        //public DbSet<Despacho> Despachos { get; set;}

        // public DbSet<ProcessoExpImp> ProcessoExpImps { get; set; }

        // public DbSet <Fronteira> Fronteiras { get; set; }

        // public DbSet <Documento> Documentos { get; set; }

        // public DbSet <EmbarqueRodoviario> EmbarqueRodoviarios { get; set; }

       /* protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UsuarioProcesso>()
                .HasKey(pe => new { pe.UsuarioId, pe.ProcessoId});
            modelBuilder.Entity<UsuarioProcesso>()
                .HasOne(p => p.Usuario)
                .WithMany(pe => pe.UsuariosProcessos)
                .HasForeignKey(p => p.UsuarioId);
            modelBuilder.Entity<ProcessoUsuario>()
                .HasOne(e => e.Processo)
                .WithMany(pe => pe.ProcessosUsuarios)
                .HasForeignKey(e => e.UsuarioId);

        }
       */
    }
}
