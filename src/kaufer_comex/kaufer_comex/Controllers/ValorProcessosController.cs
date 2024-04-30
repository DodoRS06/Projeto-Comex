using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class ValoresProcessosController : Controller
    {
        private readonly AppDbContext _context;

        public ValoresProcessosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.ValorProcessos.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
			ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");
			return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ValorProcesso valorprocesso)
        {
            if (ModelState.IsValid)
            {
                _context.ValorProcessos.Add(valorprocesso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");

			return View(valorprocesso);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos.FindAsync(id);
            if (dados == null)
                return NotFound();

			ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");

			return View(dados);

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ValorProcesso valorprocesso)
        {
            if (id != valorprocesso.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.ValorProcessos.Update(valorprocesso);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			ViewData["ProcessoId"] = new SelectList(_context.Processos, "Id", "CodProcessoExportacao");

			return View();
        }
       
                public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos
				.Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id); ;

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos
				.Include(d => d.Processo)
				.FirstOrDefaultAsync(d => d.Id == id); ;

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ValorProcessos.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.ValorProcessos.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
