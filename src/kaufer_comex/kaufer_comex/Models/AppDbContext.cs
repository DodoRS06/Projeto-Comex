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

        public DbSet<DCE> DCEs { get; set; }

        public DbSet<ValorProcesso> ValorProcessos { get; set; }

        public DbSet<FornecedorServico> FornecedorServicos { get; set; }

        public DbSet<CadastroDespesa> CadastroDespesas { get; set; }

        public DbSet<ExpImp> ExpImps { get; set; }

        public DbSet<Despacho> Despachos { get; set;}

        public DbSet<Destino> Destinos { get; set; }

        public DbSet<Status> Status { get; set; }

         public DbSet <Fronteira> Fronteiras { get; set; }

         public DbSet <Documento> Documentos { get; set; }
public DbSet <EmbarqueRodoviario> EmbarqueRodoviarios { get; set; }

        // public DbSet <Nota> Notas { get; set; } 

        //public DbSet <Item> Itens { get; set } 

        // public DbSet

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //base.onModelCreating(modelBuilder);
        //model.Builder.Entity<NotaItem>()
        //  .HasKey(pe => new {pe.NotasId, pe.ItemId});
        //model.Builder.Entity<NotaItem>()
        //   .HasOne(p => p.Nota)
        //   .WithMany(pe =>pe.NotaItem)
        //   .HasForeignKey(p => p.NotaId)
        //model.Builder.Entity<NotaItem>()
        //   .model.Builder.Entity<NotaItem>()
        //   .HasOne(e => e.Item)
        //   .WithMany(pe => pe.NotaItem)
        //   .HasForeignKey(e => e.ItemId);
        //}
        public DbSet <ProcessoExpImp> ProcessosExpImp  { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             base.OnModelCreating(modelBuilder);

             modelBuilder.Entity<ProcessoExpImp>()
                 .HasKey(p => new { p.ProcessoId, p.ExpImpId });

             modelBuilder.Entity<ProcessoExpImp>()
                 .HasOne(p => p.Processo).WithMany(p => p.ExpImps)
                 .HasForeignKey(p => p.ProcessoId);

             modelBuilder.Entity<ProcessoExpImp>()
                  .HasOne(p => p.ExpImp).WithMany(p => p.ProcessoExpImps)
                  .HasForeignKey(p => p.ExpImpId);

         } 
   
    }
}
