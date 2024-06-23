using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class FronteirasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorService _error;
        public FronteirasController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Fronteiras
                    .OrderBy(f => f.NomeFronteira)
                    .ToListAsync();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Fronteira fronteira)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    var fronteiraExistente = await _context.Fronteiras
                        .AnyAsync(f => f.NomeFronteira == fronteira.NomeFronteira);

                    if (fronteiraExistente)
                    {
                        ModelState.AddModelError("Fronteira", "Essa fronteira já está cadastrada.");
                        return View(fronteira);
                    }
                    _context.Fronteiras.Add(fronteira);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(fronteira);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return _error.NotFoundError();
                }

                var dados = await _context.Fronteiras.FindAsync(id);
                if (dados == null)
                {
                    return _error.NotFoundError();
                }
                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Fronteira fronteira)
        {
            try
            {
                if (id != fronteira.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    var fronteiraExistente = await _context.Fronteiras
                        .AnyAsync(f => f.NomeFronteira == fronteira.NomeFronteira && f.Id != fronteira.Id);

                    if (fronteiraExistente)
                    {
                        TempData["MensagemErro"] = "Essa fronteira já está cadastrada.";
                        return View(fronteira);
                    }

                    _context.Fronteiras.Update(fronteira);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(fronteira);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Fronteiras.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch { return _error.InternalServerError(); }
        }

        public async Task<IActionResult> Delete(int? id)
        {

            try
            {
                if (id == null)
                    return _error.NotFoundError();
                if (User.IsInRole("Admin"))
                {
                    var dados = await _context.Fronteiras.FindAsync(id);

                    if (dados == null)
                        return _error.NotFoundError();

                    return View(dados);
                }
                return _error.UnauthorizedError();
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Fronteiras.FindAsync(id);

                if (id == null)
                    return _error.NotFoundError();

                _context.Fronteiras.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["MensagemErro"] = $"Esse Fronteira está vinculado a um processo e não pode ser excluído. ";
                return View();
            }
        }
    }
}
