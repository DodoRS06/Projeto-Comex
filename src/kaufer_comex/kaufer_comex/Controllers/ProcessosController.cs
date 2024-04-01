using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Processos.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Processo processo)
        {
            if (ModelState.IsValid)
            {
                _context.Processos.Add(processo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(processo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Processo processo)
        {
            if (id != processo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Processos.Update(processo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Processos.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.Processos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
