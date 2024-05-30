using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace kaufer_comex.Controllers
{
    public class ProcessosController : Controller
    {
        private readonly AppDbContext _context;

        public ProcessosController(AppDbContext context)
        {
            _context = context;
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
                    .ToListAsync();

                foreach (var processo in dados)
                {
                   var importador = processo.ImportadorId;
                   ViewData["importador"] = GetNomeImportador(importador);
                }

                

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

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
                    return NotFound();

                InfoViewData();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
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
                    return NotFound();

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
            catch (Exception ex)
            {
                InfoViewData();
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message} Por favor, tente novamente.";
                return View();
            }
        }


        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();
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
                    return NotFound();

                ViewData["exportador"] = GetNomeExportador(dados.ExportadorId);
                ViewData["importador"] = GetNomeImportador(dados.ImportadorId);
                ViewData["destino"] = GetNomeDestino(dados.DestinoId);
                ViewData["fronteira"] = GetNomeFronteira(dados.FronteiraId);
                ViewData["responsavel"] = GetNomeResponsavel(dados.UsuarioId);
                ViewData["despachante"] = GetNomeDespachante(dados.DespachanteId);

                var agenteDeCarga = _context.EmbarqueRodoviarios.FirstOrDefault(e => e.ProcessoId == dados.Id);
                if (agenteDeCarga != null)
                {
                    ViewData["agenteDeCarga"] = GetNomeAgenteDeCarga(agenteDeCarga.AgenteDeCargaId);
                }


                var despesa = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.Id);
                if (despesa != null)
                {
                    ViewData["despesa"] = GetNomeDespesa(despesa.CadastroDespesaId);
                }

                var fornecedor = _context.DCEs.FirstOrDefault(e => e.ProcessoId == dados.Id);
                if (fornecedor != null)
                {
                    ViewData["fornecedor"] = GetNomeFornecedor(fornecedor.FornecedorServicoId);
                }

                var notaEmbarque = _context.EmbarqueRodoviarios.FirstOrDefault(e => e.ProcessoId == dados.Id);

                if (notaEmbarque != null)
                {
                    var notaProcesso = _context.Notas.FirstOrDefault(n => n.EmbarqueRodoviarioId == notaEmbarque.Id);

                    ViewData["motorista"] = GetNomeMotorista(notaProcesso.Id);
                }
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
                    Vendedor = dados.Vendedor,
                    Status = dados.Status,
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
                    Notas = new List<Nota>()

                };

                if (notaEmbarque != null)
                {
                    ViewData["EmbarqueId"] = notaEmbarque.Id;
                    view.Notas = _context.Notas.Where(n => n.EmbarqueRodoviarioId == notaEmbarque.Id).ToList();
                }

                return View(view);

            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}. Por favor, tente novamente.";
                return View();
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

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

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
                    return NotFound();

                var exportador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ExportadorId);

                ViewData["exportador"] = exportador.Nome;

                var importador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ImportadorId);

                ViewData["importador"] = importador.Nome;

                return View(dados);
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado: {ex.Message}. Por favor, tente novamente.";
                return View();
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
                    return NotFound();

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
                    return NotFound();

                var exportador_ = await _context.ProcessosExpImp
                            .FirstOrDefaultAsync(e => e.ProcessoId == id && e.ExpImp.TipoExpImp == TipoExpImp.Exportador);

                if (exportador_ == null) return NotFound();

                _context.ProcessosExpImp.Remove(exportador_);
                await _context.SaveChangesAsync();

                var importador_ = await _context.ProcessosExpImp
                            .FirstOrDefaultAsync(i => i.ProcessoId == id && i.ExpImp.TipoExpImp == TipoExpImp.Importador);

                if (importador_ == null) return NotFound();

                _context.ProcessosExpImp.Remove(importador_);
                await _context.SaveChangesAsync();

                _context.Processos.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }

        }

    }
}
