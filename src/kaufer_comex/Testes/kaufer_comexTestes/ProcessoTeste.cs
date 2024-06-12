using DocumentFormat.OpenXml.Drawing.Charts;
using kaufer_comex.Controllers;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Testes
{
    public class ProcessoTeste 
    {
        private readonly AppDbContext _dbContext;
        private readonly ProcessosController _controller;


        public void Dispose()
        {
            _dbContext.Dispose();
        }

        //public ProcessosTestes()
        //{
        //    // Configuração do contexto de banco de dados em memória
        //    var options = new DbContextOptionsBuilder<AppDbContext>()
        //        .UseInMemoryDatabase(databaseName: "kaufer_comex")
        //        .Options;

        //    _dbContext = new AppDbContext(options);
        //    _controller = new ProcessosController(_dbContext);


        //}

        [Fact]
        public async Task Create_Processo_ValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var model = new Processo
            {
                Id = 1,
                CodProcessoExportacao = "123456",
                ExportadorId = 1,
                ImportadorId = 2,
                Modal = 0,
                Incoterm = 0,
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

            var createdModel = await _dbContext.Processos.FirstOrDefaultAsync(p => p.CodProcessoExportacao == model.CodProcessoExportacao);
            Assert.NotNull(createdModel);
            Assert.Equal(model.CodProcessoExportacao, createdModel.CodProcessoExportacao);
            Assert.Equal(model.ExportadorId, createdModel.ExportadorId);
            Assert.Equal(model.ImportadorId, createdModel.ImportadorId);
        }

        [Fact]
        public async Task Create_Processo_DuplicateCodProcessoExportacao_ReturnsViewWithModelError()
        {
            // Arrange
            var model = new Processo
            {
                CodProcessoExportacao = "123456"
            };

            // Adicionando um processo existente para simular duplicidade
            _dbContext.Processos.Add(new Processo { CodProcessoExportacao = "123456" });
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _controller.Create(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
            Assert.True(_controller.ModelState.ContainsKey("CodProcessoExportacao"));
        }
    }
}