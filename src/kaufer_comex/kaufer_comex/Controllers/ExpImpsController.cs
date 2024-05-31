using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class ExpImpsController : Controller
    {
        private readonly AppDbContext _context;

        public ExpImpsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.ExpImps
				.OrderBy(e => e.Nome)
				.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExpImp expimp)
        {
            if (ModelState.IsValid)
            {
                _context.ExpImps.Add(expimp);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(expimp);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ExpImps.FindAsync(id);
            if (dados == null)
                return NotFound();

            return View(dados);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExpImp expimp)
        {
            if (id != expimp.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.ExpImps.Update(expimp);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ExpImps.FindAsync(id);

            if (id == null)
                return NotFound();

            return View(dados);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var dados = await _context.ExpImps.FindAsync(id);

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

            var dados = await _context.ExpImps.FindAsync(id);

            if (id == null)
                return NotFound();

            _context.ExpImps.Remove(dados);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }


    }
}
