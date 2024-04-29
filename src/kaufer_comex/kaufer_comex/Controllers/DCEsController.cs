using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class DCEsController : Controller
    {

        private readonly AppDbContext _context;

        public DCEsController(AppDbContext context)
        {
            _context = context;
        }

		private async Task Dropdowns(DCE dce = null)
		{

			ViewData["CadastroDespesaId"] = new SelectList(_context.CadastroDespesas, "Id", "NomeDespesa", dce?.CadastroDespesaId);
			ViewData["FornecedorServicoId"] = new SelectList(_context.FornecedorServicos, "Id", "Nome", dce?.FornecedorServicoId);
		}

		public async Task<IActionResult> Index()
        {
            var dados = await _context.DCEs
                .Include(p => p.CadastroDespesas)
                .Include(p => p.FornecedorServicos)
                .ToListAsync();

			await Dropdowns();

			return View(dados);
        }

        public IActionResult Create()
        {

			Dropdowns();

			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DCE dce)
        {
            if (ModelState.IsValid)
            {
                _context.DCEs.Add(dce);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			await Dropdowns(dce);

			return View(dce);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);
            if (dados == null)
                return NotFound();

			await Dropdowns(dados);

			return View(dados);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DCE dce)
        {
            if (id != dce.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.DCEs.Update(dce);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

			await Dropdowns(dce);

			return View(dce);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);

            if (id == null)
                return NotFound();

			await Dropdowns(dados);

			return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);

            if (id == null)
                return NotFound();

			await Dropdowns(dados);

			return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.DCEs.FindAsync(id);

            if (id == null)
                return NotFound();

			await Dropdowns(dados);

			_context.DCEs.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
