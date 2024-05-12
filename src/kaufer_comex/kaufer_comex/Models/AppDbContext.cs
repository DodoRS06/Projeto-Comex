using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace kaufer_comex.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Processo> Processos { get; set; }

        public DbSet<AgenteDeCarga> AgenteDeCargas { get; set; }

        public DbSet<Despachante> Despachantes { get; set; }

        public DbSet<Vendedor> Vendedores { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<DCE> DCEs { get; set; }

        public DbSet<ValorProcesso> ValorProcessos { get; set; }

        public DbSet<FornecedorServico> FornecedorServicos { get; set; }

        public DbSet<CadastroDespesa> CadastroDespesas { get; set; }

        public DbSet<ExpImp> ExpImps { get; set; }

        public DbSet<Despacho> Despachos { get; set; }

        public DbSet<Destino> Destinos { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Fronteira> Fronteiras { get; set; }

        public DbSet<Documento> Documentos { get; set; }

        public DbSet<EmbarqueRodoviario> EmbarqueRodoviarios { get; set; }

        public DbSet<Nota> Notas { get; set; }

        public DbSet<Item> Itens { get; set; }

        public DbSet<Veiculo> Veiculos { get; set; }

        public DbSet<NotaItem> NotaItens { get; set; }

        public DbSet<NotaItemTemp> NotaItemTemps { get; set; }

        public DbSet<ProcessoExpImp> ProcessosExpImp { get; set; }

		public DbSet<FornecedorServicoDCE> FornecedorServicoDCEs { get; set; }

		public DbSet<CadastroDespesaDCE> CadastroDespesaDCEs { get; set; }

       

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

           modelBuilder.Entity<NotaItem>()
                 .HasKey(pe => new { pe.NotaId, pe.ItemId });

            modelBuilder.Entity<NotaItem>()
                 .HasOne(p => p.Nota)
                 .WithMany(pe => pe.NotaItem)
                 .HasForeignKey(p => p.NotaId);

            modelBuilder.Entity<NotaItem>()
                 .HasOne(e => e.Item)
                 .WithMany(pe => pe.NotaItem)
                 .HasForeignKey(e => e.ItemId);

			modelBuilder.Entity<FornecedorServicoDCE>()
				 .HasKey(p => new { p.FornecedorServicoId, p.DCEId });

			modelBuilder.Entity<FornecedorServicoDCE>()
				.HasOne(p => p.FornecedorServico).WithMany(p => p.FornecedorServicoDCEs)
				.HasForeignKey(p => p.FornecedorServicoId);

			modelBuilder.Entity<FornecedorServicoDCE>()
				.HasOne(p => p.DCE).WithMany(p => p.FornecedorServicos)
				.HasForeignKey(p => p.DCEId);

			modelBuilder.Entity<CadastroDespesaDCE>()
				 .HasKey(p => new { p.CadastroDespesaId, p.DCEId });

			modelBuilder.Entity<CadastroDespesaDCE>()
				.HasOne(p => p.CadastroDespesa).WithMany(p => p.CadastroDespesaDCEs)
				.HasForeignKey(p => p.CadastroDespesaId);

			modelBuilder.Entity<CadastroDespesaDCE>()
				.HasOne(p => p.DCE).WithMany(p => p.CadastroDespesas)
				.HasForeignKey(p => p.DCEId);

            modelBuilder.Entity<DCE>()
                .HasOne(d => d.Processo)
                .WithMany(p => p.DCES)
                .HasForeignKey(d => d.ProcessoId);
        }

    }
}
