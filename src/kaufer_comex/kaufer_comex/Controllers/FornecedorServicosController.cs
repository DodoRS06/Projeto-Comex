using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class FornecedorServicosController : Controller
    {

        private readonly AppDbContext _context;

        public FornecedorServicosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dados = await _context.FornecedorServicos.ToListAsync();

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FornecedorServico fornecedorservico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.FornecedorServicos.Add(fornecedorservico);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(fornecedorservico);
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

                var dados = await _context.FornecedorServicos.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, FornecedorServico fornecedorservico)
        {
            try
            {
                if (id != fornecedorservico.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.FornecedorServicos.Update(fornecedorservico);
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

                var dados = await _context.FornecedorServicos.FindAsync(id);

                if (id == null)
                    return NotFound();

                return View(dados);
            }
            catch {
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

                var dados = await _context.FornecedorServicos.FindAsync(id);

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

                var dados = await _context.FornecedorServicos.FindAsync(id);

                if (id == null)
                    return NotFound();

                _context.FornecedorServicos.Remove(dados);
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
