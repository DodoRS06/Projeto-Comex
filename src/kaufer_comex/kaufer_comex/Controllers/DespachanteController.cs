using kaufer_comex.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace kaufer_comex.Controllers
{
    [Authorize]
    public class Despachantes : Controller
    {
        private readonly AppDbContext _context;

        public Despachantes(AppDbContext context)
        {
            _context = context;
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

                var dados = await _context.Despachantes.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, Despachante despachante)
        {
            try
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
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
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
            try
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
