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

        public Vendedores(AppDbContext context)
        {
            _context = context;
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
            catch
            {
                TempData["MensagemErro"] = $"Erro ao carregar os dados. Tente novamente";
                return View();
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Vendedores.FindAsync(id);
                if (dados == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();

            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Vendedor vendedor)
        {
            try
            {
                if (id != vendedor.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.Vendedores.Update(vendedor);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Vendedores.FindAsync(id);

                if (id == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }


        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Vendedores.FindAsync(id);

                if (id == null)
                    return NotFound();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Vendedores.FindAsync(id);

                if (id == null)
                    return NotFound();

                _context.Vendedores.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch { }
            TempData["MensagemErro"] = $"Esse vendedor está vinculado a um processo. Não pode ser excluído";
            return View();
        }
    }
}

