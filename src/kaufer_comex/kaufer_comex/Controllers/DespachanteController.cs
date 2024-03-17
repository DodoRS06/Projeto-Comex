using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class Despachantes : Controller
    {
        private readonly AppDbContext _context;

        public Despachantes(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var dados = await _context.Despachantes.ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Despachante Despachante)
        {
            if (ModelState.IsValid)
            {
                _context.Despachantes.Add(Despachante);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(Despachante);
        }
    }
}
