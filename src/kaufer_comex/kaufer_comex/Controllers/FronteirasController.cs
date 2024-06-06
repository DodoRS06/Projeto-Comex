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
        public FronteirasController(AppDbContext context)
        {
            _context = context;
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
                        TempData["MensagemErro"] = $"Essa fronteira já está cadastrada.";
                        return View(fronteira);
                    }
                    _context.Fronteiras.Add(fronteira);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(fronteira);
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

                var dados = await _context.Fronteiras.FindAsync(id);

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Esse destino já está cadastrado .";
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Fronteira fronteira)
        {
            try
            {
                if (id != fronteira.Id)
                    return NotFound();
                if (ModelState.IsValid)
                {
                    _context.Fronteiras.Update(fronteira);
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

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)

                return NotFound();
            var dados = await _context.Fronteiras.FindAsync(id);

            if (dados == null)

                return NotFound();

            return View(dados);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)

                return NotFound();
            var dados = await _context.Fronteiras.FindAsync(id);

            if (dados == null)

                return NotFound();

            return View(dados);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)

                    return NotFound();

                var dados = await _context.Fronteiras.FindAsync(id);

                if (dados == null)

                    return NotFound();
                _context.Fronteiras.Remove(dados);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch
            {
                TempData["MensagemErro"] = $"Essa fronteira está vinculada a um processo. Não pode ser excluída";
                return View();
            }
        }
    }
}
