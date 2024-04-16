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
            var dados = await _context.Processos
                .Include(p => p.Despachante)
                .Include(p => p.Vendedor)
                .Include(p => p.Destino)
                .Include(p => p.Fronteira)
                .Include(p => p.Status)
                .Include(p => p.ExpImps)
                .ThenInclude( p => p.ExpImp)
                .ToListAsync();
            return View(dados);
        }

        // GET: Processos/Create
        public IActionResult Create()
        {
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "NomePais");
            ViewData["FronteiraId"] = new SelectList(_context.Fronteiras, "Id", "NomeFronteira");
            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
            ViewData["DespachanteId"] = new SelectList(_context.Despachantes, "Id", "NomeDespachante");
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "NomeVendedor");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "StatusAtual");

            var importador = _context.ExpImps.Where(i => i.TipoExpImp == TipoExpImp.Importador).ToList();
            var exportador = _context.ExpImps.Where(e => e.TipoExpImp == TipoExpImp.Exportador).ToList();

            ViewData["Importador"] = new SelectList(importador, "Id", "Nome");
            ViewData["Exportador"] = new SelectList(exportador, "Id", "Nome");

            return View();
        }

        //POST: Processos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Processo processo)
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

            var importador = _context.ExpImps.Where(i => i.TipoExpImp == TipoExpImp.Importador).ToList();
            var exportador = _context.ExpImps.Where(e => e.TipoExpImp == TipoExpImp.Exportador).ToList();

            ViewData["Importador"] = new SelectList(importador, "Id", "Nome");
            ViewData["Exportador"] = new SelectList(exportador, "Id", "Nome");

           

            return View(processo);
        }

        // GET: Processos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos
                .Include(p => p.Despachante)
                .Include(p => p.Vendedor)
                .Include(p => p.Destino)
                .Include(p => p.Fronteira)
                .Include(p => p.Status)
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

            var importador = _context.ExpImps.Where(i => i.TipoExpImp == TipoExpImp.Importador).ToList();
            var exportador = _context.ExpImps.Where(e => e.TipoExpImp == TipoExpImp.Exportador).ToList();

            ViewData["Importador"] = new SelectList(importador, "Id", "Nome");
            ViewData["Exportador"] = new SelectList(exportador, "Id", "Nome");


            return View(dados);

        }

        // POST: Processos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Processo processo)
        {
            if (id != processo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Processos.Update(processo);
                await _context.SaveChangesAsync();

                var importador_ = new ProcessoExpImp
                {
                    ProcessoId = processo.Id,
                    ExpImpId = processo.ImportadorId
                };

                _context.ProcessosExpImp.Update(importador_);
                await _context.SaveChangesAsync();

                var exportador_ = new ProcessoExpImp
                {
                    ProcessoId = processo.Id,
                    ExpImpId = processo.ExportadorId
                };

                _context.ProcessosExpImp.Update(exportador_);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewData["DestinoId"] = new SelectList(_context.Destinos, "Id", "NomePais");
            ViewData["FronteiraId"] = new SelectList(_context.Fronteiras, "Id", "NomeFronteira");
            ViewData["AgenteDeCargaId"] = new SelectList(_context.AgenteDeCargas, "Id", "NomeAgenteCarga");
            ViewData["DespachanteId"] = new SelectList(_context.Despachantes, "Id", "NomeDespachante");
            ViewData["VendedorId"] = new SelectList(_context.Vendedores, "Id", "NomeVendedor");
            ViewData["StatusId"] = new SelectList(_context.Status, "Id", "StatusAtual");

            var importador = _context.ExpImps.Where(i => i.TipoExpImp == TipoExpImp.Importador).ToList();
            var exportador = _context.ExpImps.Where(e => e.TipoExpImp == TipoExpImp.Exportador).ToList();

            ViewData["Importador"] = new SelectList(importador, "Id", "Nome");
            ViewData["Exportador"] = new SelectList(exportador, "Id", "Nome");


            return View();
        }

        // GET: Processos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos
                .Include(p => p.Despachante)
                .Include(p => p.Vendedor)
                .Include(p => p.Destino)
                .Include(p => p.Fronteira)
                .Include(p => p.Status)
                .Include(p => p.ExpImps)
                .ThenInclude(p => p.ExpImp)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        // GET: Processos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos
                
                .Include(p => p.Despachante)
                .Include(p => p.Vendedor)
                .Include(p => p.Destino)
                .Include(p => p.Fronteira)
                .Include(p => p.Status)
                .Include(p => p.ExpImps)
                .ThenInclude(p => p.ExpImp)
                .FirstOrDefaultAsync(p => p.Id == id); 

            if (id == null)
                return NotFound();

            return View(dados);
        }

        // POST: Processos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos
                
                .Include(p => p.Despachante)
                .Include(p => p.Vendedor)
                .Include(p => p.Destino)
                .Include(p => p.Fronteira)
                .Include(p => p.Status)
                .Include(p => p.ExpImps)
                .ThenInclude(p => p.ExpImp)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (id == null)
                return NotFound();

            _context.Processos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }
}
