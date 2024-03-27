using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class DestinosController : Controller
    {
        private readonly AppDbContext _context;

        public DestinosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Destinos.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Destino destino)
        {
            if (ModelState.IsValid)
            {
                _context.Destinos.Add(destino);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(destino);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Destinos.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Destino destino)
        {
            if (id != destino.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Destinos.Update(destino);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Destinos.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Destinos.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Destinos.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.Destinos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}

