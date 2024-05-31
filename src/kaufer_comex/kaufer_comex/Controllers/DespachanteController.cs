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
            var dados = await _context.Despachantes
                 .OrderBy(a => a.NomeDespachante)
                 .ToListAsync();

            return View(dados);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Despachante despachante)
        {
            if (ModelState.IsValid)
            {
                var agenteExistente = await _context.Despachantes
                    .AnyAsync(a => a.NomeDespachante == despachante.NomeDespachante);

                if (agenteExistente)
                {
                    ModelState.AddModelError("NomeDespachante", "Já existe um despachante com esse nome.");
                    return View(despachante);
                }
                
                _context.Despachantes.Add(despachante);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(despachante);
        }
    

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var dados = await _context.Despachantes.FindAsync(id);
        if (dados == null)
            return NotFound();

        return View(dados);

    }
    [HttpPost]
    public async Task<IActionResult> Edit(int id, Despachante despachante)
    {
        if (id != despachante.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Despachantes.Update(despachante);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View();
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var dados = await _context.Despachantes.FindAsync(id);

        if (id == null)
            return NotFound();

        return View(dados);
    }


    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var dados = await _context.Despachantes.FindAsync(id);

        if (id == null)
            return NotFound();

        return View(dados);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        if (id == null)
            return NotFound();

        var dados = await _context.Despachantes.FindAsync(id);

        if (id == null)
            return NotFound();

        _context.Despachantes.Remove(dados);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");

    }
}
}
