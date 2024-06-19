using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class ItensController : Controller
    {
        private readonly AppDbContext _context;

        private readonly ErrorService _error;
        public ItensController(AppDbContext context, ErrorService error)
        {
            _context = context;
            _error = error; 
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.Itens
                        .OrderBy(a => a.DescricaoProduto)
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Item item)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var ItemExistente = await _context.Itens
                      .AnyAsync(a => a.DescricaoProduto == item.DescricaoProduto);


                    if (ItemExistente)
                    {
                        ModelState.AddModelError("DescricaoProduto", "Esse Item já está cadastrado.");
                        return View(item);
                    }
                    _context.Itens.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");

                }
                return View(item);
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

                var dados = await _context.Itens.FindAsync(id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Item item)
        {
            try
            {
                if (id != item.Id)
                    return _error.NotFoundError();

                if (ModelState.IsValid)
                {
                    _context.Itens.Update(item);
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

                var dados = await _context.Itens.FindAsync(id);

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

                var dados = await _context.Itens.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                return View(dados);
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return _error.NotFoundError();

                var dados = await _context.Itens.FindAsync(id);

                if (dados == null)
                    return _error.NotFoundError();

                _context.Itens.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return _error.InternalServerError();
            }
        }
    }
}
