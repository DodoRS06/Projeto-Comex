using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
                    .ThenInclude(p => p.ExpImp)
                    .FirstOrDefaultAsync(p => p.Id == id);

                var exportador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ExportadorId);

                ViewData["exportador"] = exportador.Nome;

                var importador = _context.ExpImps.FirstOrDefault(e => e.Id == dados.ImportadorId);

                ViewData["importador"] = importador.Nome;


                if (dados == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
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
