using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class DespachosController : Controller
    {
        private readonly AppDbContext _context;

        public DespachosController(AppDbContext context)
        {
            _context = context;
        }

        // GET : Despachos
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Despachos
                    .Include(d => d.Processo)
                    .ToListAsync();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // GET : Despachos/Create
        public IActionResult Create()
        {
            try
            {
                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
                return View();
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // POST : Despachos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Despacho despacho)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Despachos.Add(despacho);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
                return View(despacho);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        // GET: Despachos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Despachos.FindAsync(id);
                if (dados == null)
                    return NotFound();

                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }

        }

        // POST: Despachos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Despacho despacho)
        {
            try
            {
                if (id != despacho.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.Despachos.Update(despacho);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
                return View();
            }
            catch 
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View(despacho);
            }
        }

        // GET : Despachos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Despachos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

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

        // GET: Despachos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Despachos
                    .Include(d => d.Processo)
                    .FirstOrDefaultAsync(d => d.Id == id);

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

        // POST: Despachos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Despachos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                _context.Despachos.Remove(dados);
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
