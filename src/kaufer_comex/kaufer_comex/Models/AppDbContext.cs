using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace kaufer_comex.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       public DbSet<Processo> Processos { get; set; }

        public DbSet<AgenteDeCarga> AgenteDeCargas{ get; set; }

        public DbSet<Despachante> Despachantes { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        //public DbSet<DCE> DCEs { get; set; }

        public DbSet<ValorProcesso> ValorProcessos { get; set; }

        //public DbSet<FornecedorServico> FornecedorServicos { get; set; }

        //public DbSet<CadastroDespesa> CadastroDespesas { get; set; }

        //public DbSet<ExpImp> ExpImps { get; set; }

        //public DbSet<UsuarioProcesso> UsuariosProcesso{ get; set; }

        public DbSet<Despacho> Despachos { get; set;}

        public DbSet<Destino> Destinos { get; set; }

        public DbSet<Status> Status { get; set; }

         public DbSet <Fronteira> Fronteiras { get; set; }

        // public DbSet <Documento> Documentos { get; set; }

        // public DbSet <EmbarqueRodoviario> EmbarqueRodoviarios { get; set; }

       /* public DbSet <ProcessoExpImp> ProcessosExpImp  { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);
             //modelBuilder.Entity<UsuarioProcesso>()
             //    .HasKey(pe => new { pe.UsuarioId, pe.ProcessoId});
             //modelBuilder.Entity<UsuarioProcesso>()
             //    .HasOne(p => p.Usuario)
             //    .WithMany(pe => pe.UsuariosProcessos)
             //    .HasForeignKey(p => p.UsuarioId);
             //modelBuilder.Entity<ProcessoUsuario>()
             //    .HasOne(e => e.Processo)
             //    .WithMany(pe => pe.ProcessosUsuarios)
             //    .HasForeignKey(e => e.ProcessoId);

             modelBuilder.Entity<ProcessoExpImp>()
                 .HasKey(p => new { p.ProcessoId, p.ExpImpId });

             modelBuilder.Entity<ProcessoExpImp>()
                 .HasOne(p => p.Processo).WithMany(p => p.ExpImps)
                 .HasForeignKey(p => p.ProcessoId);

             modelBuilder.Entity<ProcessoExpImp>()
                  .HasOne(p => p.ExpImp).WithMany(p => p.Processos)
                  .HasForeignKey(p => p.ExpImpId);

         } */
   
    }
}
