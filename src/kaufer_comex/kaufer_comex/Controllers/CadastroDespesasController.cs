using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    [Authorize]
    public class CadastroDespesasController : Controller
    {

        private readonly AppDbContext _context;

        public CadastroDespesasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.CadastroDespesas.ToListAsync();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Erro ao carregar os dados. Tente novamente.";
                return View();
            }
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CadastroDespesa cadastrodespesa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.CadastroDespesas.Add(cadastrodespesa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(cadastrodespesa);
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

                var dados = await _context.CadastroDespesas.FindAsync(id);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CadastroDespesa cadastrodespesa)
        {
            try
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

                var dados = await _context.CadastroDespesas.FindAsync(id);

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

                var dados = await _context.CadastroDespesas.FindAsync(id);

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }

        }
    }
}
