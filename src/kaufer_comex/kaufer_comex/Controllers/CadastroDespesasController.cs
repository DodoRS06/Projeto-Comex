using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class CadastroDespesasController : Controller
    {

        private readonly AppDbContext _context;

        public CadastroDespesasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.CadastroDespesas.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CadastroDespesa cadastrodespesa)
        {
            if (ModelState.IsValid)
            {
                _context.CadastroDespesas.Add(cadastrodespesa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(cadastrodespesa);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.CadastroDespesas.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CadastroDespesa cadastrodespesa)
        {
            if (id != cadastrodespesa.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.CadastroDespesas.Update(cadastrodespesa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.CadastroDespesas.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.CadastroDespesas.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.CadastroDespesas.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.CadastroDespesas.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
