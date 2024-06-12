using DocumentFormat.OpenXml.Drawing.Charts;
using kaufer_comex.Controllers;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Diagnostics;
using System.Linq.Expressions;

namespace kaufer_comex.Testes
{
    public class ProcessoTeste 
    {
        private readonly AppDbContext _dbContext;
        private readonly ProcessosController _controller;

        public ProcessoTeste()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(databaseName: "kaufer_comex")
        .Options;

            _dbContext = new AppDbContext(options);
            _controller = new ProcessosController(_dbContext);

            KauferDatabase();
        }

        [Fact]
        public void Dispose()
        {
            _dbContext.Dispose();
        }

        private void KauferDatabase()
        {
            // Inserir dados de teste no banco de dados
            var processos = new[]
            {
            new Processo {
                Id = 1,
                CodProcessoExportacao = "123456",
                ExportadorId = 1,
                ImportadorId = 2,
                Modal = Modal.Rodoviario,
                Incoterm = Incoterm.DAT,
                DestinoId = 1,
                UsuarioId = 2,
                DespachanteId = 2,
                FronteiraId = 2,
                VendedorId = 0,
                StatusId = 0,
                Proforma = "73839",
                DataInicioProcesso = DateTime.Now,
                PrevisaoProducao = DateTime.Now,
                PrevisaoPagamento = DateTime.Now,
                PrevisaoCruze = DateTime.Now,
                PrevisaoColeta = DateTime.Now,
                PrevisaoEntrega = DateTime.Now,
                PedidosRelacionados = "",
                Observacoes = ""  },
            new Processo {
                Id = 2,
                CodProcessoExportacao = "78963",
                ExportadorId = 1,
                ImportadorId = 2,
                Modal = Modal.Maritimo,
                Incoterm = Incoterm.CIP,
                DestinoId = 1,
                UsuarioId = 2,
                DespachanteId = 2,
                FronteiraId = 2,
                VendedorId = 0,
                StatusId = 0,
                Proforma = "73839",
                DataInicioProcesso = DateTime.Now,
                PrevisaoProducao = DateTime.Now,
                PrevisaoPagamento = DateTime.Now,
                PrevisaoCruze = DateTime.Now,
                PrevisaoColeta = DateTime.Now,
                PrevisaoEntrega = DateTime.Now,
                PedidosRelacionados = "",
                Observacoes = ""},
            new Processo {
                 Id = 3,
                CodProcessoExportacao = "876/23",
                ExportadorId = 1,
                ImportadorId = 2,
                Modal = Modal.Rodoviario,
                Incoterm = Incoterm.CFR,
                DestinoId = 1,
                UsuarioId = 2,
                DespachanteId = 2,
                FronteiraId = 2,
                VendedorId = 0,
                StatusId = 0,
                Proforma = "63820",
                DataInicioProcesso = DateTime.Now,
                PrevisaoProducao = DateTime.Now,
                PrevisaoPagamento = DateTime.Now,
                PrevisaoCruze = DateTime.Now,
                PrevisaoColeta = DateTime.Now,
                PrevisaoEntrega = DateTime.Now,
                PedidosRelacionados = "",
                Observacoes = ""}
            };

            var exportadorImportador = new[]
            {
            new ExpImp {
                Id = 1,
                Sigla = "CKW",
                TipoExpImp = TipoExpImp.Exportador,
                Nome = "Kaufer",
                Endereco = "Rua Oliveira Monteiro",
                Cidade = "São Paulo",
                Estado = "SP",
                Pais = "Brasil",
                Cep = "12358-789",
                Telefone = "(11) 98652-2628",
                Email = "kaufer@gmail.com",
                Cnpj = "00.000.000/0000-00",
                Contato = "kaufer@gmail.com",
                Observacoes = ""

                },
            new ExpImp {
                Id = 2,
                Sigla = "JCWP",
                TipoExpImp = TipoExpImp.Importador,
                Nome = "JCWP SA",
                Endereco = "Rua Obere Schwendi",
                Cidade = "Teufen",
                Estado = "AR",
                Pais = "SWITZERLAND",
                Cep = "22222-789",
                Telefone = "(11) 12311-2628",
                Email = "jcwp@gmail.com",
                Cnpj = "00.000.000/0000-00",
                Contato = "jcwp@gmail.com",
                Observacoes = ""
                 }
            };

            var processoExpImp = new List<ProcessoExpImp>
            {
                new ProcessoExpImp { ProcessoId = 1, ExpImpId = 1 },
                new ProcessoExpImp { ProcessoId = 1, ExpImpId = 2 },
                new ProcessoExpImp { ProcessoId = 2, ExpImpId = 1 },
                new ProcessoExpImp { ProcessoId = 2, ExpImpId = 2 }
            };

            _dbContext.Processos.AddRange(processos);
            _dbContext.ExpImps.AddRange(exportadorImportador);
            _dbContext.ProcessosExpImp.AddRange(processoExpImp);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Create_Processo_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new Processo
            {
                Id = 3,
                CodProcessoExportacao = "123456",
                ExportadorId = 1,
                ImportadorId = 2,
                Modal = Modal.Maritimo,
                Incoterm = Incoterm.CFR,
                DestinoId = 1,
                UsuarioId = 2,
                DespachanteId = 2,
                FronteiraId = 2,
                VendedorId = 0,
                StatusId = 0,
                Proforma = "73839",
                DataInicioProcesso = DateTime.Now,
                PrevisaoProducao = DateTime.Now,
                PrevisaoPagamento = DateTime.Now,
                PrevisaoCruze = DateTime.Now,
                PrevisaoColeta = DateTime.Now,
                PrevisaoEntrega = DateTime.Now,
                PedidosRelacionados = "",
                Observacoes = ""
            };

            // Act
            var result = await _controller.Create(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

        }

        [Fact]
            public async Task Create_Processo_DuplicateCodProcessoExportacao_ReturnsViewWithModelError()
            {
                // Arrange
                var model = new Processo
                {
                    Id = 1,
                    CodProcessoExportacao = "123456",
                    ExportadorId = 1,
                    ImportadorId = 2,
                    Modal = Modal.Rodoviario,
                    Incoterm = Incoterm.DAT,
                    DestinoId = 1,
                    UsuarioId = 2,
                    DespachanteId = 2,
                    FronteiraId = 2,
                    VendedorId = 0,
                    StatusId = 0,
                    Proforma = "73839",
                    DataInicioProcesso = DateTime.Now,
                    PrevisaoProducao = DateTime.Now,
                    PrevisaoPagamento = DateTime.Now,
                    PrevisaoCruze = DateTime.Now,
                    PrevisaoColeta = DateTime.Now,
                    PrevisaoEntrega = DateTime.Now,
                    PedidosRelacionados = "",
                    Observacoes = ""
                };

            _dbContext.Processos.Add(model);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Create(model);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.True(_controller.ModelState.ContainsKey("CodProcessoExportacao"));
        }
        }
    }