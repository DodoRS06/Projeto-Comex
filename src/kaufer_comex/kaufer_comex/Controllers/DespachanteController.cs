using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class Despachantes : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public Despachantes(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Despachantes
                     .OrderBy(a => a.NomeDespachante)
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
        public async Task<IActionResult> Create(Despachante despachante)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var agenteExistente = await _context.Despachantes
                        .AnyAsync(a => a.NomeDespachante == despachante.NomeDespachante);

                    if (agenteExistente)
                    {
                        TempData["MensagemErro"] = $"Esse despachante já está cadastrado .";
                        return View(despachante);
                    }

                    _context.Despachantes.Add(despachante);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(despachante);
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
                    return _error.NotFoundError();

                var dados = await _context.Despachantes.FindAsync(id);
                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Despachante despachante)
        {
            try
            {
                if (id != despachante.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.Despachantes.Update(despachante);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
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

                var dados = await _context.Despachantes.FindAsync(id);

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
                    var dados = await _context.Despachantes.FindAsync(id);

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

                var dados = await _context.Despachantes.FindAsync(id);

                if (id == null)
                    return _error.NotFoundError();

                _context.Despachantes.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["MensagemErro"] = $"Despachante está vinculado a um processo. Não pode ser excluído.";
                return View();
            }
            catch (Exception)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado.";
                return View();
            }
        }
    }
}
