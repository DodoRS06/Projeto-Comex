using kaufer_comex.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace kaufer_comex.Controllers
{
    public class DestinosController : Controller
    {
        private readonly AppDbContext _context;

        public DestinosController(AppDbContext context)
        {
            _context = context;
        }
        //GET: Destinos/Index
        public async Task<IActionResult> Index()
        {
            try
            {

                var dados = await _context.Destinos
                    .OrderBy(a => a.NomePais)
                    .ToListAsync();

                return View(dados);
            }
            catch
            {
                TempData["MensagemErro"] = $"Erro ao carregar os dados. Tente novamente";
                return View();
            }

        }

        //GET: Destinos/Create
        public IActionResult Create()
        {

            return View();
        }


        //POST: Destinos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Destino destino)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var destinoExistente = await _context.Destinos
                   .AnyAsync(a => a.NomePais == destino.NomePais);

                    if (destinoExistente)
                    {
                        ModelState.AddModelError("NomePais", "Esse destino já está cadastrado.");
                        return View(destino);
                    }
                    _context.Destinos.Add(destino);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(destino);
            }
            catch
            {
                TempData["MensagemErro"] = $"Ocorreu um erro inesperado. Por favor, tente novamente.";
                return View();
            }
        }

        //GET: Destinos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Destinos.FindAsync(id);
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


        //POST: Destinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Destino destino)
        {
            try
            {
                if (id != destino.Id)
                    return NotFound();

                if (ModelState.IsValid)
                {
                    _context.Destinos.Update(destino);
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

        //GET: Destinos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Destinos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                return View(dados);
            }
            catch { return NotFound(); }
        }

        //GET: Destinos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Destinos.FindAsync(id);

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


        //POST: Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            try
            {
                if (id == null)
                    return NotFound();

                var dados = await _context.Destinos.FindAsync(id);

                if (dados == null)
                    return NotFound();

                _context.Destinos.Remove(dados);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["MensagemErro"] = $"Destino está vinculado a um processo. Não pode ser excluído";
                return View();
            }
        }
    }
}

