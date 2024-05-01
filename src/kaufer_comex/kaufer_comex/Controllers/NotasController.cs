using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class NotasController : Controller
    {
        private AppDbContext _context;

        public NotasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Notas
                .Include(p => p.Veiculo)
                .ToListAsync();

            return View(dados);
        }

        public IActionResult Create()
        {
            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
            ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
            ViewData["EmbarqueRodoviarioId"] = new SelectList(_context.EmbarqueRodoviarios, "Id", "Transportadora");

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Nota nota)
        {
            if (ModelState.IsValid)
            {

                _context.Notas.Add(nota);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");
            ViewData["NotaItem"] = new SelectList(_context.Itens, "Id", "DescricaoProduto");
            ViewData["EmbarqueRodoviarioId"] = new SelectList(_context.EmbarqueRodoviarios, "Id", "Transportadora");

            return View(nota);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
                return NotFound();

            var dados = await _context.Notas
                .Include(p => p.Veiculo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dados == null)
                return NotFound();

            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");

            return View(dados);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Nota nota)
        {
            if (id != nota.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Notas.Update(nota);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }

            ViewData["VeiculoId"] = new SelectList(_context.Veiculos, "Id", "Motorista");

            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Notas
                .Include(p => p.Veiculo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Notas
                .Include(p => p.Veiculo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dados == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.Notas
                .Include(p => p.Veiculo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dados == null)
                return NotFound();

            _context.Notas.Remove(dados);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
