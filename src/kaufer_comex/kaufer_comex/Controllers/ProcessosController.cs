using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class ProcessosController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public ProcessosController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        // GET: Processos
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Processos
                    .Include(p => p.Despachante)
                    .Include(p => p.Vendedor)
                    .Include(p => p.Destino)
                    .Include(p => p.Fronteira)
                    .Include(p => p.Status)
                    .Include(p => p.Usuario)
                    .Include(p => p.ExpImps)
                    .ThenInclude(p => p.ExpImp)
                    .OrderByDescending(p => p.Id)
                    .ToListAsync();

                var importadores = new Dictionary<int, string>();

                foreach (var processo in dados)
                {
                    var importador = await _context.ExpImps
                        .FirstOrDefaultAsync(i => i.Id == processo.ImportadorId);

                    if (importador != null)
                    {
                        importadores[processo.Id] = GetNomeImportador(importador.Id);
                    }
                }

                ViewData["Importadores"] = importadores;

                return View(dados);
            }
			catch (SqlException)
			{
				TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Processos.";
				return _error.InternalServerError();
			}
			catch (InvalidOperationException)
			{
				TempData["MensagemErro"] = $"Erro ao recuperar Processos do banco de dados.";
				return _error.BadRequestError();
			}
			catch (Exception)
			{
				TempData["MensagemErro"] = $"Erro ao recuperar Processos do banco de dados.";
				return _error.InternalServerError();
			}
		}

        // GET: Processos/Create
        public IActionResult Create()
        {
            try
            {
                InfoViewData();
                return View();
            }
            catch(Exception)
            {
                return _error.InternalServerError();
            }
        }

        //POST: Processos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Processo processo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var processoExistente = await _context.Processos
                       .AnyAsync(a => a.CodProcessoExportacao == processo.CodProcessoExportacao);

                    if (processoExistente)
                    {
                        ModelState.AddModelError("CodProcessoExportacao", "Esse número de processo já está cadastrado.");
                        InfoViewData();
                        return View(processo);
                    }

                    _context.Processos.Add(processo);
                    await _context.SaveChangesAsync();

                    var importador_ = new ProcessoExpImp
                    {
                        ProcessoId = processo.Id,
                        ExpImpId = processo.ImportadorId
                    };

                    _context.ProcessosExpImp.Add(importador_);
                    await _context.SaveChangesAsync();

                    var exportador_ = new ProcessoExpImp
                    {
                        ProcessoId = processo.Id,
                        ExpImpId = processo.ExportadorId
                    };

                    _context.ProcessosExpImp.Add(exportador_);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

                InfoViewData();

                return View(processo);
            }
            catch (DbUpdateException)
            {
				TempData["MensagemErro"] = $"Erro ao cadastrar Processo.";
				return _error.ConflictError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Processos
                    .Include(p => p.Despachante)
                    .Include(p => p.Vendedor)
                    .Include(p => p.Destino)
                    .Include(p => p.Fronteira)
                    .Include(p => p.Status)
                    .Include(p => p.Usuario)
                    .Include(p => p.ExpImps)
                    .ThenInclude(p => p.ExpImp)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                InfoViewData();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }

        }
        private void InfoViewData()
        {
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "NomePais");
            ViewData["FronteiraId"] = new SelectList(_context.Fronteiras, "Id", "NomeFronteira");
            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
            ViewData["DespachanteId"] = new SelectList(_context.Despachantes, "Id", "NomeDespachante");
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "NomeVendedor");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "StatusAtual");
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "NomeFuncionario");

            var importador = _context.ExpImps.Where(i => i.TipoExpImp == TipoExpImp.Importador).ToList();
            var exportador = _context.ExpImps.Where(e => e.TipoExpImp == TipoExpImp.Exportador).ToList();

            ViewData["Importador"] = new SelectList(importador, "Id", "Nome");
            ViewData["Exportador"] = new SelectList(exportador, "Id", "Nome");
        }

        // POST: Processos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Processo processo)
        {
            try
            {
                if (id != processo.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
 
                    var processoAntigo = await _context.Processos
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id);

                    var exportadorMudado = processoAntigo.ExportadorId != processo.ExportadorId;
                    var importadorMudado = processoAntigo.ImportadorId != processo.ImportadorId;

                    _context.Processos.Update(processo);
                    await _context.SaveChangesAsync();

                    // Se houver alteração no exportador
                    if (exportadorMudado)
                    {
                        var exportadorExpImp = await _context.ProcessosExpImp
                            .FirstOrDefaultAsync(e => e.ProcessoId == id && e.ExpImp.TipoExpImp == TipoExpImp.Exportador);

                        if (exportadorExpImp != null)
                        {
                            _context.ProcessosExpImp.Remove(exportadorExpImp);
                            await _context.SaveChangesAsync();

                            var novoExportadorExpImp = new ProcessoExpImp
                            {
                                ProcessoId = id,
                                ExpImpId = processo.ExportadorId
                            };

                            _context.ProcessosExpImp.Add(novoExportadorExpImp);

                            //exportadorExpImp.ExpImpId = processo.ExportadorId;
                        }
                    }

                    // Se houver alteração no importador
                    if (importadorMudado)
                    {
                        var importadorExpImp = await _context.ProcessosExpImp
                            .FirstOrDefaultAsync(i => i.ProcessoId == id && i.ExpImp.TipoExpImp == TipoExpImp.Importador);

                        if (importadorExpImp != null)
                        {
                            _context.ProcessosExpImp.Remove(importadorExpImp);
                            await _context.SaveChangesAsync();

                            var novoImportadorExpImp = new ProcessoExpImp
                            {
                                ProcessoId = id,
                                ExpImpId = processo.ImportadorId
                            };

                            _context.ProcessosExpImp.Add(novoImportadorExpImp);

                            //importadorExpImp.ExpImpId = processo.ImportadorId;
                        }
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                InfoViewData();
                return View();
            }
			catch (DbUpdateException)
			{
				TempData["MensagemErro"] = $"Erro ao editar Processo.";
				return _error.ConflictError();
			}
			catch (Exception)
            {
                InfoViewData();
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return _error.InternalServerError();
            }
		}

            // GET: Processos/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                var dados = await _context.Processos
                   .Include(p => p.Despachante)
                   .Include(p => p.Vendedor)
                   .Include(p => p.Destino)
                   .Include(p => p.Fronteira)
                   .Include(p => p.Status)
                   .Include(p => p.Usuario)
                   .Include(p => p.ExpImps)
                   .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                ViewData["exportador"] = GetNomeExportador(dados.ExportadorId);
                ViewData["importador"] = GetNomeImportador(dados.ImportadorId);
                ViewData["destino"] = GetNomeDestino(dados.DestinoId);
                ViewData["fronteira"] = GetNomeFronteira(dados.FronteiraId);
                ViewData["responsavel"] = GetNomeResponsavel(dados.UsuarioId);
                ViewData["despachante"] = GetNomeDespachante(dados.DespachanteId);
                ViewData["vendedor"] = GetNomeVendedor(dados.VendedorId);
                ViewData["status"] = GetStatus(dados.StatusId);

                var DCE = await _context.DCEs.Where(d => d.ProcessoId == dados.Id).ToListAsync();
                if (DCE != null)
                {
                    ViewData["ValorTotal"] = await _context.DCEs.Where(d => d.ProcessoId == dados.Id).SumAsync(d => (decimal)d.Valor);
                }

                var agenteDeCarga = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.ProcessoId == dados.Id);
                if (agenteDeCarga != null)
                {
                    ViewData["agenteDeCarga"] = GetNomeAgenteDeCarga(agenteDeCarga.AgenteDeCargaId);
                }


                var despesa = await _context.DCEs.FirstOrDefaultAsync(e => e.ProcessoId == dados.Id);
                if (despesa != null)
                {
                    ViewData["despesa"] = GetNomeDespesa(despesa.CadastroDespesaId);
                }

                var fornecedor = await _context.DCEs.FirstOrDefaultAsync(e => e.ProcessoId == dados.Id);
                if (fornecedor != null)
                {
                    ViewData["fornecedor"] = GetNomeFornecedor(fornecedor.FornecedorServicoId);
                }

                var notaEmbarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.ProcessoId == dados.Id);


                var view = new DetalhesProcessoView
                {
                    Id = dados.Id,
                    CodProcessoExportacao = dados.CodProcessoExportacao,
                    ExportadorId = dados.ExportadorId,
                    ImportadorId = dados.ImportadorId,
                    Modal = dados.Modal,
                    Incoterm = dados.Incoterm,
                    DestinoId = dados.DestinoId,
                    UsuarioId = dados.UsuarioId,
                    DespachanteId = dados.DespachanteId,
                    FronteiraId = dados.FronteiraId,
                    VendedorId = dados.VendedorId,
                    StatusId = dados.StatusId,
                    Proforma = dados.Proforma,
                    DataInicioProcesso = dados.DataInicioProcesso,
                    PrevisaoProducao = dados.PrevisaoProducao,
                    PrevisaoPagamento = dados.PrevisaoPagamento,
                    PrevisaoCruze = dados.PrevisaoCruze,
                    PrevisaoColeta = dados.PrevisaoColeta,
                    PrevisaoEntrega = dados.PrevisaoEntrega,
                    PedidosRelacionados = dados.PedidosRelacionados,
                    Observacoes = dados.Observacoes,
                    Despachos = _context.Despachos.Where(d => d.ProcessoId == dados.Id).ToList(),
                    Documentos = _context.Documentos.Where(d => d.ProcessoId == dados.Id).ToList(),
                    EmbarquesRodoviarios = _context.EmbarqueRodoviarios.Where(d => d.ProcessoId == dados.Id).ToList(),
                    DCES = _context.DCEs.Where(d => d.ProcessoId == dados.Id).ToList(),
                    ValorProcessos = _context.ValorProcessos.Where(v => v.ProcessoId == dados.Id).ToList(),
                    Veiculos = _context.Veiculos.Where(v => v.ProcessoId == dados.Id).ToList(),
                    Notas = new List<Nota>(),


                };

                if (notaEmbarque != null)
                {
                    ViewData["EmbarqueId"] = notaEmbarque.Id;
                    view.Notas = _context.Notas.Where(n => n.EmbarqueRodoviarioId == notaEmbarque.Id).ToList();

                    foreach (var nota in view.Notas)
                    {
                        var notaProcesso = _context.Notas.FirstOrDefault(n => n.EmbarqueRodoviarioId == notaEmbarque.Id);
                        var veiculo = notaProcesso.VeiculoId;
                        ViewData["motorista"] = GetNomeMotorista(veiculo);
                    }
                }

                return View(view);

            }
			catch (SqlException ex)
			{
				TempData["MensagemErro"] = $"Erro de conexão com o banco de dados ao recuperar Processo. {ex.Message}";
				return _error.InternalServerError();
			}
			catch (InvalidOperationException ex)
			{
				TempData["MensagemErro"] = $"Erro ao recuperar Processo do banco de dados. {ex.Message}";
				return _error.BadRequestError();
			}
			catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}. Por favor, tente novamente.";
                return _error.InternalServerError();
            }
        }

        private string GetNomeExportador(int? id) => id != null ? _context.ExpImps.FirstOrDefault(e => e.Id == id)?.Nome : string.Empty;
        private string GetNomeImportador(int? id) => id != null ? _context.ExpImps.FirstOrDefault(i => i.Id == id)?.Nome : string.Empty;
        private string GetNomeDestino(int? id) => id != null ? _context.Destinos.FirstOrDefault(d => d.Id == id)?.NomePais : string.Empty;
        private string GetNomeFronteira(int? id) => id != null ? _context.Fronteiras.FirstOrDefault(f => f.Id == id)?.NomeFronteira : string.Empty;
        private string GetNomeResponsavel(int? id) => id != null ? _context.Usuarios.FirstOrDefault(u => u.Id == id)?.NomeFuncionario : string.Empty;
        private string GetNomeDespachante(int? id) => id != null ? _context.Despachantes.FirstOrDefault(d => d.Id == id)?.NomeDespachante : string.Empty;
        private string GetNomeAgenteDeCarga(int? id) => id != null ? _context.AgenteDeCargas.FirstOrDefault(d => d.Id == id)?.NomeAgenteCarga : string.Empty;
        private string GetNomeDespesa(int? id) => id != null ? _context.CadastroDespesas.FirstOrDefault(d => d.Id == id)?.NomeDespesa : string.Empty;
        private string GetNomeFornecedor(int? id) => id != null ? _context.FornecedorServicos.FirstOrDefault(d => d.Id == id)?.Nome : string.Empty;
        private string GetNomeMotorista(int? id) => id != null ? _context.Veiculos.FirstOrDefault(d => d.Id == id)?.Motorista : string.Empty;
        private string GetNomeVendedor(int? id) => id != null ? _context.Vendedores.FirstOrDefault(d => d.Id == id)?.NomeVendedor : string.Empty;
        private string GetStatus(int? id) => id != null ? _context.Status.FirstOrDefault(d => d.Id == id)?.StatusAtual : string.Empty;



        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.Processos

                    .Include(p => p.Despachante)
                    .Include(p => p.Vendedor)
                    .Include(p => p.Destino)
                    .Include(p => p.Fronteira)
                    .Include(p => p.Status)
                    .Include(p => p.Usuario)
                    .Include(p => p.ExpImps)
                    .ThenInclude(p => p.ExpImp)
                    .FirstOrDefaultAsync(p => p.Id == id);

                    if (dados == null)
                        return _error.NotFoundError();

                    var exportador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ExportadorId);

                    ViewData["exportador"] = exportador.Nome;

                    var importador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ImportadorId);

                    ViewData["importador"] = importador.Nome;

                    return View(dados);
                }
                return _error.UnauthorizedError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Processos
                    .Include(p => p.Despachante)
                    .Include(p => p.Vendedor)
                    .Include(p => p.Usuario)
                    .Include(p => p.Destino)
                    .Include(p => p.Fronteira)
                    .Include(p => p.Status)
                    .Include(p => p.Usuario)
                    .Include(p => p.ExpImps)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (dados == null)
                    return _error.NotFoundError();

                if (User.IsInRole("Admin"))
                {

                    var embarque = await _context.EmbarqueRodoviarios.FirstOrDefaultAsync(e => e.ProcessoId == dados.Id);
                    if (embarque != null)
                    {
                        var notas = await _context.Notas.Where(n => n.EmbarqueRodoviarioId == embarque.Id).ToListAsync();
                        foreach (var nota in notas)
                        {
                            _context.Notas.Remove(nota);
                            await _context.SaveChangesAsync();

                            var notaItem = await _context.NotaItens.Where(ni => ni.NotaId == nota.Id).ToListAsync();

                            foreach (var item in notaItem)
                            {
                                _context.NotaItens.Remove(item);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }

                    var valores = await _context.ValorProcessos.FirstOrDefaultAsync(p => p.ProcessoId == dados.Id);
                    if (valores != null)
                    {
                        _context.ValorProcessos.Remove(valores);
                        await _context.SaveChangesAsync();
                    }

                    var exportador_ = await _context.ProcessosExpImp
                                .FirstOrDefaultAsync(e => e.ProcessoId == id && e.ExpImp.TipoExpImp == TipoExpImp.Exportador);

                    if (exportador_ != null)
                    {
                        _context.ProcessosExpImp.Remove(exportador_);
                        await _context.SaveChangesAsync();
                    }

                    var importador_ = await _context.ProcessosExpImp
                                .FirstOrDefaultAsync(i => i.ProcessoId == id && i.ExpImp.TipoExpImp == TipoExpImp.Importador);

                    if (importador_ != null)
                    {
                        _context.ProcessosExpImp.Remove(importador_);
                        await _context.SaveChangesAsync();

                    }

                    _context.Processos.Remove(dados);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return _error.UnauthorizedError();
            }
			catch (DbUpdateException)
			{
				TempData["MensagemErro"] = $"Erro ao excluir Processo.";
				return _error.ConflictError();
			}
			catch (Exception)
            {
				TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
				return _error.InternalServerError();
            }

        }

        [HttpGet]
        public async Task<FileResult> ExportProcessosExcel()
        {
            var processos = await _context.Processos
                     .Include(p => p.Despachante)
                     .Include(p => p.Vendedor)
                     .Include(p => p.Destino)
                     .Include(p => p.Fronteira)
                     .Include(p => p.Status)
                     .Include(p => p.Usuario)
                     .Include(p => p.ExpImps)
                     .ThenInclude(p => p.ExpImp)
                     .Include(p => p.Veiculos)
                     .Include(p => p.Documento)
                     .Include(p => p.Despacho)
                     .Include(p => p.ValorProcesso)
                     .Include(p => p.EmbarqueRodoviario)
                     .ThenInclude(p => p.Notas)
                     .Include(p => p.DCES)
                     .ThenInclude(p => p.CadastroDespesas)
             .ToListAsync();


            var fileName = "Processo.xlsx";

            return GenerateExcel(fileName, processos);
        }

        private FileResult GenerateExcel(string fileName, IEnumerable<Processo> processos)
        {
            DataTable dataTable = new DataTable("Processos");

            // Colunas fixas
            var fixedColumns = new DataColumn[]
            {
            // Processo
               new DataColumn("Id"),
               new DataColumn("CodProcessoExportacao"),
               new DataColumn("Exportador"),
               new DataColumn("Importador"),
               new DataColumn("Usuário Responsável"),
               new DataColumn("Modal"),
               new DataColumn("Incoterm"),
               new DataColumn("Destino"),
               new DataColumn("Fronteira"),
               new DataColumn("Despachante"),
               new DataColumn("Vendedor"),
               new DataColumn("Status"),
               new DataColumn("Proforma"),
               new DataColumn("DataInicioProcesso"),
               new DataColumn("PrevisaoProducao"),
               new DataColumn("PrevisaoPagamento"),
               new DataColumn("PrevisaoColeta"),
               new DataColumn("Previsão Cruze"),
               new DataColumn("Previsão de entrega"),
               new DataColumn("Observacoes"),
               new DataColumn("PedidosRelacionados"),
               // Embarque
               new DataColumn("Embarque Rodoviário"),
               new DataColumn("Transportadora"),
               new DataColumn("Data do Embarque"),
               new DataColumn("Transit Time"),
               new DataColumn("Chegada no Destino"),
               new DataColumn("Booking"),
               new DataColumn("Deadline Draft"),
               new DataColumn("Deadline VGM"),
               new DataColumn("Deadline Carga"),
               new DataColumn("Agente de Carga"),
               // Despacho
               new DataColumn("Despacho"),
               new DataColumn("Número DUE"),
               new DataColumn("Data DUE"),
               new DataColumn("Data de Exportação"),
               new DataColumn("Conhecimento de Embarque"),
               new DataColumn("Data de Conhecimento"),
               new DataColumn("Tipo"),
               new DataColumn("Data da Averbação"),
               new DataColumn("Código do País"),
               new DataColumn("Parametrização"),
               // Documento
               new DataColumn("Documento"),
               new DataColumn("Certificado de origem"),
               new DataColumn("Certificado de Seguro"),
               new DataColumn("Data Envio"),
               new DataColumn("Tracking"),
               new DataColumn("Courier"),
               // Valor do Processo
               new DataColumn("Valor Processo"),
               new DataColumn("Moeda"),
               new DataColumn("Valor Fob/Fca"),
               new DataColumn("Frete Internacional"),
               new DataColumn("Seguro Internacional"),
               new DataColumn("Valor Total"),

            };

            dataTable.Columns.AddRange(fixedColumns);

            // Encontrar o número máximo de veículos em um processo
            int maxVeiculos = processos.Max(p => p.Veiculos?.Count ?? 0);

            // Adicionar colunas dinâmicas para os veículos
            for (int i = 0; i < maxVeiculos; i++)
            {
                dataTable.Columns.Add(new DataColumn($"Veiculo_{i + 1}"));
                dataTable.Columns.Add(new DataColumn($"Veiculo_{i + 1}_Placa"));
                dataTable.Columns.Add(new DataColumn($"Veiculo_{i + 1}_Motorista"));
            }

            int maxNota = processos.Max(p => p.EmbarqueRodoviario.Notas?.Count ?? 0);

            // Adicionar colunas dinâmicas para as Notas
            for (int i = 0; i < maxNota; i++)
            {
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_NumeroNf"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_Emissao"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_ValorFob"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_ValorFrete"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_ValorSeguro"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_ValorCif"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_PesoLiq"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_PesoBruto"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_TaxaCambial"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_CertificadoQualidade"));
                dataTable.Columns.Add(new DataColumn($"Nota_{i + 1}_Veiculo"));


            }

            int maxDCE = processos.Max(p => p.DCES?.Count ?? 0);

            // Adicionar colunas dinâmicas para as DCES
            for (int i = 0; i < maxDCE; i++)
            {
                dataTable.Columns.Add(new DataColumn($"DCE_{i + 1}"));
                dataTable.Columns.Add(new DataColumn($"DCE_{i + 1}_CadastroDespesaNome"));
                dataTable.Columns.Add(new DataColumn($"DCE_{i + 1}_Valor"));
                dataTable.Columns.Add(new DataColumn($"DCE_{i + 1}_FornecedorServicoNome"));
                dataTable.Columns.Add(new DataColumn($"DCE_{i + 1}_Observacao"));


            }

            // Preencher as linhas da tabela
            foreach (var processo in processos)
            {
                var row = dataTable.NewRow();

                // Preencher colunas fixas
                // Processo
                row["Id"] = processo.Id;
                row["CodProcessoExportacao"] = processo.CodProcessoExportacao;
                row["Exportador"] = GetNomeExportador(processo.ExportadorId);
                row["Importador"] = GetNomeImportador(processo.ImportadorId);
                row["Usuário Responsável"] = processo.Usuario.NomeFuncionario;
                row["Modal"] = processo.Modal;
                row["Incoterm"] = processo.Incoterm;
                row["Destino"] = processo.Destino.NomePais;
                row["Fronteira"] = processo.Fronteira.NomeFronteira;
                row["Despachante"] = processo.Despachante.NomeDespachante;
                row["Vendedor"] = processo.Vendedor.NomeVendedor;
                row["Status"] = processo.Status.StatusAtual;
                row["Proforma"] = processo.Proforma;
                row["DataInicioProcesso"] = processo.DataInicioProcesso;
                row["PrevisaoProducao"] = processo.PrevisaoProducao;
                row["PrevisaoPagamento"] = processo.PrevisaoPagamento;
                row["PrevisaoColeta"] = processo.PrevisaoColeta;
                row["Previsão Cruze"] = processo.PrevisaoCruze;
                row["Previsão de entrega"] = processo.PrevisaoEntrega;
                row["Observacoes"] = processo.Observacoes;
                row["PedidosRelacionados"] = processo.PedidosRelacionados;
                // Embarque
                if (processo.EmbarqueRodoviario != null)
                {
                    row["Embarque Rodoviário"] = processo.EmbarqueRodoviario.Id;
                    row["Transportadora"] = processo.EmbarqueRodoviario.Transportadora;
                    row["Data do Embarque"] = processo.EmbarqueRodoviario.DataEmbarque;
                    row["Transit Time"] = processo.EmbarqueRodoviario.TransitTime;
                    row["Chegada no Destino"] = processo.EmbarqueRodoviario.ChegadaDestino;
                    row["Booking"] = processo.EmbarqueRodoviario.Booking;
                    row["Deadline Draft"] = processo.EmbarqueRodoviario.DeadlineDraft;
                    row["Deadline VGM"] = processo.EmbarqueRodoviario.DeadlineVgm;
                    row["Deadline Carga"] = processo.EmbarqueRodoviario.DeadlineCarga;
                    row["Agente de Carga"] = GetNomeAgenteDeCarga(processo.EmbarqueRodoviario.AgenteDeCargaId);
                }

                // Despacho
                if (processo.Despacho != null)
                {
                    row["Despacho"] = processo.Despacho.Id;
                    row["Número DUE"] = processo.Despacho.NumeroDue;
                    row["Data DUE"] = processo.Despacho.DataDue;
                    row["Data de Exportação"] = processo.Despacho.DataExportacao;
                    row["Conhecimento de Embarque"] = processo.Despacho.ConhecimentoEmbarque;
                    row["Data de Conhecimento"] = processo.Despacho.DataConhecimento;
                    row["Tipo"] = processo.Despacho.Tipo;
                    row["Data da Averbação"] = processo.Despacho.DataAverbacao;
                    row["Código do País"] = processo.Despacho.CodPais;
                    row["Parametrização"] = processo.Despacho.Parametrizacao;
                }
                // Documento
                if (processo.Documento != null)
                {
                    row["Documento"] = processo.Documento.Id;
                    row["Certificado de origem"] = processo.Documento.CertificadoOrigem;
                    row["Certificado de Seguro"] = processo.Documento.CertificadoSeguro;
                    row["Data Envio"] = processo.Documento.DataEnvioOrigem;
                    row["Tracking"] = processo.Documento.TrackinCourier;
                    row["Courier"] = processo.Documento.Courier;
                }
                // Valor de Processo
                if (processo.ValorProcesso != null)
                {
                    row["Valor Processo"] = processo.ValorProcesso.Id;
                    row["Moeda"] = processo.ValorProcesso.Moeda;
                    row["Valor Fob/Fca"] = processo.ValorProcesso.ValorFobFca;
                    row["Frete Internacional"] = processo.ValorProcesso.FreteInternacional;
                    row["Seguro Internacional"] = processo.ValorProcesso.SeguroInternaciona;
                    row["Valor Total"] = processo.ValorProcesso.ValorTotalCif;
                }

                // Preencher colunas dinâmicas para os veículos
                if (processo.Veiculos != null)
                {
                    for (int i = 0; i < processo.Veiculos.Count; i++)
                    {
                        row[$"Veiculo_{i + 1}"] = processo.Veiculos[i].Id;
                        row[$"Veiculo_{i + 1}_Placa"] = processo.Veiculos[i].Placa;
                        row[$"Veiculo_{i + 1}_Motorista"] = processo.Veiculos[i].Motorista;
                    }
                }

                if (processo.EmbarqueRodoviario.Notas != null)
                {
                    for (int i = 0; i < processo.EmbarqueRodoviario.Notas.Count; i++)
                    {
                        row[$"Nota_{i + 1}"] = processo.EmbarqueRodoviario.Notas[i].Id;
                        row[$"Nota_{i + 1}_NumeroNf"] = processo.EmbarqueRodoviario.Notas[i].NumeroNf;
                        row[$"Nota_{i + 1}_ValorFob"] = processo.EmbarqueRodoviario.Notas[i].ValorFob;
                        row[$"Nota_{i + 1}_ValorFrete"] = processo.EmbarqueRodoviario.Notas[i].ValorFrete;
                        row[$"Nota_{i + 1}_ValorSeguro"] = processo.EmbarqueRodoviario.Notas[i].ValorSeguro;
                        row[$"Nota_{i + 1}_ValorCif"] = processo.EmbarqueRodoviario.Notas[i].ValorCif;
                        row[$"Nota_{i + 1}_PesoLiq"] = processo.EmbarqueRodoviario.Notas[i].PesoLiq;
                        row[$"Nota_{i + 1}_PesoBruto"] = processo.EmbarqueRodoviario.Notas[i].PesoBruto;
                        row[$"Nota_{i + 1}_TaxaCambial"] = processo.EmbarqueRodoviario.Notas[i].TaxaCambial;
                        row[$"Nota_{i + 1}_CertificadoQualidade"] = processo.EmbarqueRodoviario.Notas[i].CertificadoQualidade;
                        row[$"Nota_{i + 1}_Veiculo"] = processo.EmbarqueRodoviario.Notas[i].Veiculo.Motorista;

                    }
                }

                if (processo.DCES != null)
                {
                    for (int i = 0; i < processo.DCES.Count; i++)
                    {
                        row[$"DCE_{i + 1}"] = processo.DCES[i].Id;
                        row[$"DCE_{i + 1}_CadastroDespesaNome"] = GetNomeDespesa(processo.DCES[i].CadastroDespesaId);
                        row[$"DCE_{i + 1}_Valor"] = processo.DCES[i].Valor;
                        row[$"DCE_{i + 1}_FornecedorServicoNome"] = GetNomeFornecedor(processo.DCES[i].FornecedorServicoId);
                        row[$"DCE_{i + 1}_Observacao"] = processo.DCES[i].Observacao;

                    }
                }

                dataTable.Rows.Add(row);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable, "Processo");

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);

                }
            }
        }
    }
}
