using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
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
            var dados = await _context.Despachos
                .Include(d => d.Processo)
                .ToListAsync();

            return View(dados);
        }

        // GET : Despachos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST : Despachos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Despacho despacho)
        {
            if (ModelState.IsValid)
            {
                _context.Despachos.Add(despacho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(despacho);
        }

        // GET: Despachos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Despachos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }

        // POST: Despachos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Despacho despacho)
        {
            if (id != despacho.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Despachos.Update(despacho);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET : Despachos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Despachos
                .Include(d => d.Processo)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        // GET: Despachos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Despachos
                .Include(d => d.Processo)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        // POST: Despachos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Despachos.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.Despachos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
