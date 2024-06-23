using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class Vendedores : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;

        public Vendedores(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Vendedores
                    .OrderBy(b => b.NomeVendedor)
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
        public async Task<IActionResult> Create(Vendedor vendedor)
        {
            try
            {
                if (ModelState.IsValid)

                {

                    var vendedorExistente = await _context.Vendedores
                        .AnyAsync(b => b.NomeVendedor == vendedor.NomeVendedor);

                    if (vendedorExistente)
                    {
                        TempData["MensagemErro"] = $"Já existe vendedor com esse nome.";
                        return View(vendedor);
                    }

                    _context.Vendedores.Add(vendedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(vendedor);
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

                var dados = await _context.Vendedores.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, Vendedor vendedor)
        {
            try
            {
                if (id != vendedor.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.Vendedores.Update(vendedor);
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

                var dados = await _context.Vendedores.FindAsync(id);

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
                    var dados = await _context.Vendedores.FindAsync(id);

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

                var dados = await _context.Vendedores.FindAsync(id);

                if (id == null)
                    return _error.NotFoundError();

                _context.Vendedores.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData["MensagemErro"] = $"Vendedor está vinculado a um processo. Não pode ser excluído.";
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


